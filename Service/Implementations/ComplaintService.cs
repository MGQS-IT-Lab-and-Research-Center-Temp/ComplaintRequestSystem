using ComplaintRequestSystem.Entities;
using ComplaintRequestSystem.Models;
using ComplaintRequestSystem.Models.Complaint;
using ComplaintRequestSystem.Repository.Interfaces;
using ComplaintRequestSystem.Service.Interfaces;
using Google.Protobuf.WellKnownTypes;
using System.Linq.Expressions;
using System.Security.Claims;

namespace ComplaintRequestSystem.Service.Implementations
{
	public class ComplaintService : IComplaintService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public ComplaintService(
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseResponseModel> CreateComplaint(CreateComplaintViewModel request)
        {
            var response = new BaseResponseModel();
            var createdBy = _httpContextAccessor.HttpContext.User.Identity.Name;
            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = await _unitOfWork.Users.GetAsync(userIdClaim);

            var complaint = new Complaint
            {
                UserId = user.Id,
                ComplaintText = request.ComplaintText,
                ImageUrl = request.ImageUrl,
                CreatedBy = createdBy,
            };

           if(request.DepartmentIds is null)
           {
                response.Message = "Select one or more department(s).";
                return response;
           }

            var departments = await  _unitOfWork.Departments.GetAllByIdsAsync(request.DepartmentIds);


            var departmentComplaints = new HashSet<DepartmentComplaint>();

            foreach (var department in departments)
            {
                var departmentComplaint = new DepartmentComplaint
                {
                    DepartmentId = department.Id,
                    ComplaintId = complaint.Id,
                    Department = department,
                    Complaint = complaint,
                    CreatedBy = createdBy
                };

                departmentComplaints.Add(departmentComplaint);
            }
            complaint.DepartmentComplaint = departmentComplaints;

            try
            {
               await _unitOfWork.Complaints.CreateAsync(complaint);
               await _unitOfWork.SaveChangesAsync();
                response.Message = "Complaint created successfully!";
                response.Status = true;

                return response;
            }
            catch (Exception ex)
            {
                response.Message = $"Failed to create complaint: {ex.Message}";
                return response;
            }
        }

        public async Task<BaseResponseModel> DeleteComplaint(string complaintId)
        {
            var response = new BaseResponseModel();

            Expression<Func<Complaint, bool>> expression = (c => (c.Id == complaintId)
                                        && (c.Id == complaintId
                                        && c.IsDeleted == false
                                        && c.IsClosed == false));

            var isComplaintExist = await _unitOfWork.Complaints.ExistsAsync(expression);

            if (!isComplaintExist)
            {
                response.Message = "Complaint does not exist!";
                return response;
            }


            var complaint = await _unitOfWork.Complaints.GetAsync(complaintId);
            complaint.IsDeleted = true;

            try
            {
               await _unitOfWork.Complaints.RemoveAsync(complaint);
               await _unitOfWork.SaveChangesAsync();
                response.Message = "Complaint deleted successfully!";
                response.Status = true;

                return response;
            }
            catch (Exception ex)
            {
                response.Message = $"Complaint delete failed: {ex.Message}";
                return response;
            }
        }


        public async Task<ComplaintsResponseModel> DisplayComplaint()
        {

			var response = new ComplaintsResponseModel();

			try
			{
				var complaints = await  _unitOfWork.Complaints.GetComplaints();

				if (complaints.Count == 0)
				{
					response.Message = "No complaint found!";
					return response;
				}

				response.Data = complaints
					.Where(q => !q.IsDeleted)
					.Select(complaint => new ComplaintViewModel
					{
						Id = complaint.Id,
						UserId = complaint.UserId,
						ComplaintText = complaint.ComplaintText,
						UserName = complaint.User.UserName,
						ImageUrl = complaint.ImageUrl,
					}).ToList();

				response.Status = true;
				response.Message = "Success";
			}
			catch (Exception ex)
			{
				response.Message = $"An error occured: {ex.Message}";
				return response;
			}

			return response;
		}

        public async Task<ComplaintsResponseModel> GetAllComplaint()
        {
            var response = new ComplaintsResponseModel();

            try
            {
                var IsInRole = _httpContextAccessor.HttpContext.User.IsInRole("Admin");
                var userIdClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                Expression<Func<Complaint, bool>> expression = q => q.UserId == userIdClaim;

                var complaints = IsInRole ? await  _unitOfWork.Complaints.GetComplaints() : await _unitOfWork.Complaints.GetComplaints(expression);

                if (complaints.Count == 0)
                {
                    response.Message = "No complaint found!";
                    return response;
                }

                response.Data = complaints
                    .Where(q => q.IsDeleted == false)
                    .Select(complaint => new ComplaintViewModel
                    {
                        Id = complaint.Id,
                        ComplaintText = complaint.ComplaintText,
                        UserName = complaint.User.UserName,
                    }).ToList();

                response.Status = true;
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.Message = $"An error occured: {ex.StackTrace}";
                return response;
            }

            return response;
        }

        public async Task<ComplaintResponseModel> GetComplaint(string complaintId)
        {
            var response = new ComplaintResponseModel();
            var complaintExist = await _unitOfWork.Complaints.ExistsAsync(q => q.Id == complaintId && q.IsDeleted == false);
            var IsInRole = _httpContextAccessor.HttpContext.User.IsInRole("Admin");
            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var complaint = new Complaint();

            if (!complaintExist)
            {
                response.Message = $"Complaint with id {complaintId} does not exist!";
                return response;
            }

            complaint = IsInRole ? await  _unitOfWork.Complaints.GetComplaint(q => q.Id == complaintId && !q.IsDeleted) : await _unitOfWork.Complaints.GetComplaint(q => q.Id == complaintId
                                                 && q.UserId == userIdClaim
                                                 && !q.IsDeleted);

            if (complaint is null)
            {
                response.Message = "Complaint not found!";
                return response;
            }

            response.Message = "Success";
            response.Status = true;
            response.Data = new ComplaintViewModel
            {
                Id = complaint.Id,
                ComplaintText = complaint.ComplaintText,
                UserId = complaint.UserId,
                UserName = complaint.User.UserName,
            };

            return response;
        }

        public async Task<ComplaintsResponseModel> GetComplaintsByDepartmentId(string departmentId)
        {
            var response = new ComplaintsResponseModel();

            try
            {
                var complaints = await _unitOfWork.Complaints.GetComplaintByDepartmentId(departmentId);

                if (complaints.Count == 0)
                {
                    response.Message = "No complaint found!";
                    return response;
                }

                response.Data = complaints
									.Select(complaint => new ComplaintViewModel
                                    {
                                        Id = complaint.Id,
                                        ComplaintText = complaint.Complaint.ComplaintText,
                                        UserName = complaint.Complaint.User.UserName,
                                        ImageUrl = complaint.Complaint.ImageUrl,
                                    }).ToList();

                response.Status = true;
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                response.Message = $"An error occured: {ex.StackTrace}";
                return response;
            }

            return response;
        }


        public async Task<BaseResponseModel> UpdateComplaint(string complaintId, UpdateComplaintViewModel request)
        {
            var response = new BaseResponseModel();
            var modifiedBy = _httpContextAccessor.HttpContext.User.Identity.Name;
            var complaintExist = await _unitOfWork.Complaints.ExistsAsync(c => c.Id == complaintId);
            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = await _unitOfWork.Users.GetAsync(userIdClaim);

            if (!complaintExist)
            {
                response.Message = "Complaint does not exist!";
                return response;
            }

            var complaint = await _unitOfWork.Complaints.GetAsync(complaintId);

            if (complaint.UserId != user.Id)
            {
                response.Message = "You cannot update this complaint";
                return response;
            }

            complaint.ComplaintText = request.ComplaintText;
            complaint.ModifiedBy = modifiedBy;

            try
            {
               await _unitOfWork.Complaints.UpdateAsync(complaint);
               await _unitOfWork.SaveChangesAsync();
                response.Message = "Complaint updated successfully!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = $"Could not update the Complaint: {ex.Message}";
                return response;
            }
        }
    }
}

