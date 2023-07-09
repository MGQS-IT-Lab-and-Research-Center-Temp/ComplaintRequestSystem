using ComplaintRequestSystem.Entities;
using ComplaintRequestSystem.Models.Department;
using ComplaintRequestSystem.Models;
using ComplaintRequestSystem.Repository.Interfaces;
using ComplaintRequestSystem.Service.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace ComplaintRequestSystem.Service.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DepartmentService(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }
        public BaseResponseModel CreateDepartment(CreateDepartmentViewModel request)
        {
            var response = new BaseResponseModel();
            var createdBy = _httpContextAccessor.HttpContext.User.Identity.Name;

            var isDepartmentExist = _unitOfWork.Departments.Exists(c => c.Name == request.Name);


            if (isDepartmentExist)
            {
                response.Message = "Department already exist!";
                return response;
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                response.Message = "Department name is required!";
                return response;
            }

            var department = new Department
            {
                Name = request.Name,
                Description = request.Description,
                CreatedBy = createdBy
            };

            try
            {
                _unitOfWork.Departments.Create(department);
                _unitOfWork.SaveChanges();
                response.Status = true;
                response.Message = "Department created successfully.";

                return response;
            }
            catch (Exception ex)
            {
                response.Message = $"Failed to create department at this time: {ex.Message}";

                return response;
            }
        }

        public BaseResponseModel DeleteDepartment(string departmentId)
        {
            var response = new BaseResponseModel();
            var isDepartmentExist = _unitOfWork.Departments.Exists(c => c.Id == departmentId && !c.IsDeleted);

            if (!isDepartmentExist)
            {
                response.Message = "Department does not exist.";
                return response;
            }

            var department = _unitOfWork.Departments.Get(departmentId);
            department.IsDeleted = true;

            try
            {
                _unitOfWork.Departments.Update(department);
                _unitOfWork.SaveChanges();
                response.Status = true;
                response.Message = "Department successfully deleted.";

                return response;
            }
            catch (Exception ex)
            {
                response.Message = $"Can not delete department: {ex.Message}";
                return response;
            }
        }

        public DepartmentsResponseModel GetAllDepartment()
        {
            var response = new DepartmentsResponseModel();

            try
            {
                Expression<Func<Department, bool>> expression = c => c.IsDeleted == false;
                var department = _unitOfWork.Departments.GetAll(expression);

                if (department is null || department.Count == 0)
                {
                    response.Message = "No departments found!";
                    return response;
                }

                response.Data = department.Select(
                    department => new DepartmentViewModel
                    {
                        Id = department.Id,
                        Name = department.Name,
                        Description = department.Description
                    }).ToList();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return response;
            }

            response.Status = true;
            response.Message = "Success";

            return response;
        }

        public DepartmentResponseModel GetDepartment(string departmentId)
        {
            var response = new DepartmentResponseModel();

            Expression<Func<Department, bool>> expression = c =>
                                                (c.Id == departmentId)
                                                && (c.Id == departmentId
                                                && c.IsDeleted == false);

            var departmentExist = _unitOfWork.Departments.Exists(expression);

            if (!departmentExist)
            {
                response.Message = $"Department with id {departmentId} does not exist.";
                return response;
            }

            var department = _unitOfWork.Departments.Get(departmentId);

            response.Message = "Success";
            response.Status = true;
            response.Data = new DepartmentViewModel
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description
            };

            return response;
        }

        public IEnumerable<SelectListItem> SelectDepartment()
        {
            return _unitOfWork.Departments.SelectAll().Select(dept => new SelectListItem()
            {
                Text = dept.Name,
                Value = dept.Id
            });
        }

        public BaseResponseModel UpdateDepartment(string departmentId, UpdateDepartmentViewModel request)
        {
            var response = new BaseResponseModel();
            string modifiedBy = _httpContextAccessor.HttpContext.User.Identity.Name;
            var departmentExist = _unitOfWork.Departments.Exists(c => c.Id == departmentId);

            if (!departmentExist)
            {
                response.Message = "Department does not exist.";
                return response;
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                response.Message = "Department Name is required";
                return response;
            }

            var department = _unitOfWork.Departments.Get(departmentId);
            department.Name = request.Name; 
            department.Description = request.Description;
            department.ModifiedBy = modifiedBy;
            
            
            try 
            { 
                _unitOfWork.Departments.Update(department);
                _unitOfWork.SaveChanges();
                response.Status = true;
                response.Message = "Department updated successfully.";

                return response;
            }
            catch (Exception ex)
            {
                response.Message = $"Could not update the department: {ex.Message}";
                return response;
            }
        }
    }
}

