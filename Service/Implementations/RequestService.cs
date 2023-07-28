using ComplaintRequestSystem.Entities;
using ComplaintRequestSystem.Models.Request;
using ComplaintRequestSystem.Models;
using ComplaintRequestSystem.Repository.Interfaces;
using ComplaintRequestSystem.Service.Interfaces;
using System.Linq.Expressions;
using System.Security.Claims;
using ComplaintRequestSystem.Models.Complaint;

namespace ComplaintRequestSystem.Service.Implementations
{
    public class RequestService : IRequestService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public RequestService(
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseResponseModel> CreateRequest(CreateRequestViewModel request)
        {
            var response = new BaseResponseModel();
            var createdBy = _httpContextAccessor.HttpContext.User.Identity.Name;
            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var user =  await _unitOfWork.Users.GetAsync(userIdClaim);

            var newRequest = new Request
            {
                UserId = user.Id,
                RequestText = request.RequestText,
                ImageUrl = request.ImageUrl,
                CreatedBy = createdBy,
            };

            if (request.DepartmentIds is null)
            {
                response.Message = "Select one or more department(s).";
                return response;
            }

            var departments = await _unitOfWork.Departments.GetAllByIdsAsync(request.DepartmentIds);

            var departmentRequests = new HashSet<DepartmentRequest>();

            foreach (var department in departments)
            {
                var departmentRequest = new DepartmentRequest
                {
                    DepartmentId = department.Id,
                    RequestId = newRequest.Id,
                    Department = department,
                    Request = newRequest,
                    CreatedBy = createdBy
                };

                departmentRequests.Add(departmentRequest);
            }
            newRequest.DepartmentRequest = departmentRequests;

            try
            {
                _unitOfWork.Requests.CreateAsync(newRequest);
                _unitOfWork.SaveChangesAsync();
                response.Message = "Request created successfully!";
                response.Status = true;

                return response;
            }
            catch (Exception ex)
            {
                response.Message = $"Failed to create request: {ex.Message}";
                return response;
            }
        }

        public async Task<BaseResponseModel> DeleteRequest(string requestId)
        {
            var response = new BaseResponseModel();

            Expression<Func<Request, bool>> expression = (c => (c.Id == requestId)
                                        && (c.Id == requestId
                                        && c.IsDeleted == false
                                        && c.IsClosed == false));

            var requestExist = await  _unitOfWork.Requests.ExistsAsync(expression);

            if (!requestExist)
            {
                response.Message = "Request does not exist!";
                return response;
            }


            var request = await _unitOfWork.Requests.GetAsync(requestId);
            request.IsDeleted = true;

            try
            {
                _unitOfWork.Requests.UpdateAsync(request);
                _unitOfWork.SaveChangesAsync();
                response.Message = "Request deleted successfully!";
                response.Status = true;

                return response;
            }
            catch (Exception ex)
            {
                response.Message = $"Request delete failed: {ex.Message}";
                return response;
            }
        }

        public async Task<RequestsResponseModel> GetAllRequest()
        {
            var response = new RequestsResponseModel();

            try
            {
                var IsInRole = _httpContextAccessor.HttpContext.User.IsInRole("Admin");
                var userIdClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                Expression<Func<Request, bool>> expression = q => q.UserId == userIdClaim;

                var requests = IsInRole ? await _unitOfWork.Requests.GetRequests() : await _unitOfWork.Requests.GetRequests(expression);

                if (requests.Count == 0)
                {
                    response.Message = "No request found!";
                    return response;
                }

                response.Data = requests
                    .Where(q => q.IsDeleted == false)
                    .Select(request => new RequestViewModel
                    {
                        Id = request.Id,
                        RequestText = request.RequestText,
                        UserName = request.User.UserName,
                        ImageUrl = request.ImageUrl
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

        public async Task<RequestsResponseModel> GetRequestsByDepartmentId(string departmentId)
        {
            var response = new RequestsResponseModel();

            try
            {
                var requests = await _unitOfWork.Requests.GetRequestsByDepartmentId(departmentId);

                if (requests.Count == 0)
                {
                    response.Message = "No request found!";
                    return response;
                }

                response.Data = requests
                                    .Select(request => new RequestViewModel
                                    {
                                        Id = request.Id,
                                        RequestText = request.Request.RequestText,
                                        UserName = request.Request.User.UserName,
                                        ImageUrl = request.Request.ImageUrl
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

	    public async Task<RequestResponseModel> GetRequest(string requestId)
        {
            var response = new RequestResponseModel();
            var requestExist = await _unitOfWork.Requests.ExistsAsync(q => q.Id == requestId && q.IsDeleted == false);
            var IsInRole = _httpContextAccessor.HttpContext.User.IsInRole("Admin");
            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var request = new Request();

            if (!requestExist)
            {
                response.Message = $"Request with id {requestId} does not exist!";
                return response;
            }

            request = IsInRole ? await _unitOfWork.Requests.GetRequest(q => q.Id == requestId && !q.IsDeleted) :
                await _unitOfWork.Requests.GetRequest(q => q.Id == requestId
                                                 && q.UserId == userIdClaim
                                                 && !q.IsDeleted);

            if (request is null)
            {
                response.Message = "Request not found!";
                return response;
            }

            response.Message = "Success";
            response.Status = true;
            response.Data = new RequestViewModel
            {
                Id = request.Id,
                RequestText = request.RequestText,
                UserId = request.UserId,
                UserName = request.User.UserName,
                ImageUrl = request.ImageUrl
            };

            return response;
        }


        public async Task<BaseResponseModel> UpdateRequest(string requestId, UpdateRequestViewModel request)
        {
            var response = new BaseResponseModel();
            var modifiedBy = _httpContextAccessor.HttpContext.User.Identity.Name;
            var requestExist = await _unitOfWork.Requests.ExistsAsync(c => c.Id == requestId);
            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = _unitOfWork.Users.GetAsync(userIdClaim);

            if (!requestExist)
            {
                response.Message = "Request does not exist!";
                return response;
            }

            var newRequest = await _unitOfWork.Requests.GetAsync(requestId);

            if (newRequest.UserId != user.Id.ToString())
            {
                response.Message = "You cannot update this request";
                return response;
            }

            newRequest.RequestText = request.RequestText;
            newRequest.ModifiedBy = modifiedBy;

            try
            {
                await _unitOfWork.Requests.UpdateAsync(newRequest);
                await _unitOfWork.SaveChangesAsync();
                response.Message = "Request updated successfully!";
                response.Status = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = $"Could not update the Request: {ex.Message}";
                return response;
            }
        }

       
    }

}

