using ComplaintRequestSystem.Entities;
using ComplaintRequestSystem.Models.Request;
using ComplaintRequestSystem.Models;
using ComplaintRequestSystem.Repository.Interfaces;
using ComplaintRequestSystem.Service.Interfaces;
using System.Linq.Expressions;
using System.Security.Claims;

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
        public BaseResponseModel CreateRequest(CreateRequestViewModel request)
        {
            var response = new BaseResponseModel();
            var createdBy = _httpContextAccessor.HttpContext.User.Identity.Name;
            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = _unitOfWork.Users.Get(userIdClaim);

            var newRequest = new Request
            {
                UserId = user.Id,
                RequestText = request.RequestText,
                CreatedBy = createdBy,
            };

            var departments = _unitOfWork.Requests.GetAllByIds(request.RequestsIds);

            var departmentRequests = new HashSet<DepartmentRequest>();

            foreach (var department in departments)
            {
                var departmentRequest = new DepartmentRequest
                {
                    DepartmentId = department.Id,
                    Department = department.Department,
                    Request = newRequest,
                    CreatedBy = createdBy
                };

                departmentRequests.Add(departmentRequest);
            }
            newRequest.DepartmentRequest = departmentRequests;

            try
            {
                _unitOfWork.Requests.Create(newRequest);
                _unitOfWork.SaveChanges();
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

        public BaseResponseModel DeleteRequest(string requestId)
        {
            var response = new BaseResponseModel();

            Expression<Func<Request, bool>> expression = (c => (c.Id == requestId)
                                        && (c.Id == requestId
                                        && c.IsDeleted == false
                                        && c.IsClosed == false));

            var requestExist = _unitOfWork.Requests.Exists(expression);

            if (!requestExist)
            {
                response.Message = "Request does not exist!";
                return response;
            }


            var request = _unitOfWork.Requests.Get(requestId);
            request.IsDeleted = true;

            try
            {
                _unitOfWork.Requests.Update(request);
                _unitOfWork.SaveChanges();
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

        public RequestsResponseModel GetAllRequest()
        {
            var response = new RequestsResponseModel();

            try
            {
                var IsInRole = _httpContextAccessor.HttpContext.User.IsInRole("Admin");
                var userIdClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                Expression<Func<Request, bool>> expression = q => q.UserId == userIdClaim;

                var requests = IsInRole ? _unitOfWork.Requests.GetRequests() : _unitOfWork.Requests.GetRequests(expression);

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

        public RequestResponseModel GetRequest(string requestId)
        {
            var response = new RequestResponseModel();
            var requestExist = _unitOfWork.Requests.Exists(q => q.Id == requestId && q.IsDeleted == false);
            var IsInRole = _httpContextAccessor.HttpContext.User.IsInRole("Admin");
            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var request = new Request();

            if (!requestExist)
            {
                response.Message = $"Request with id {requestId} does not exist!";
                return response;
            }

            request = IsInRole ? _unitOfWork.Requests.GetRequest(q => q.Id == requestId && !q.IsDeleted) :
                _unitOfWork.Requests.GetRequest(q => q.Id == requestId
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
            };

            return response;
        }

        public BaseResponseModel UpdateRequest(string requestId, UpdateRequestViewModel request)
        {
            var response = new BaseResponseModel();
            var modifiedBy = _httpContextAccessor.HttpContext.User.Identity.Name;
            var requestExist = _unitOfWork.Requests.Exists(c => c.Id == requestId);
            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = _unitOfWork.Users.Get(userIdClaim);

            if (!requestExist)
            {
                response.Message = "Request does not exist!";
                return response;
            }

            var newRequest = _unitOfWork.Requests.Get(requestId);

            if (newRequest.UserId != user.Id)
            {
                response.Message = "You cannot update this request";
                return response;
            }

            newRequest.RequestText = request.RequestText;
            newRequest.ModifiedBy = modifiedBy;

            try
            {
                _unitOfWork.Requests.Update(newRequest);
                _unitOfWork.SaveChanges();
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

