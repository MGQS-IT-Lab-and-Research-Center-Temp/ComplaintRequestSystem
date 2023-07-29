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
        public async Task<BaseResponseModel> CreateDepartment(CreateDepartmentViewModel request)
        {
            var response = new BaseResponseModel();
            var createdBy = _httpContextAccessor.HttpContext.User.Identity.Name;

            var isDepartmentExist = await  _unitOfWork.Departments.ExistsAsync(c => c.Name == request.Name);


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
               await _unitOfWork.Departments.CreateAsync(department);
               await _unitOfWork.SaveChangesAsync();
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

        public async Task<BaseResponseModel> DeleteDepartment(string departmentId)
        {
            var response = new BaseResponseModel();
            var isDepartmentExist = await  _unitOfWork.Departments.ExistsAsync(c => c.Id == departmentId && !c.IsDeleted);

            if (!isDepartmentExist)
            {
                response.Message = "Department does not exist.";
                return response;
            }

            var department = await _unitOfWork.Departments.GetAsync(departmentId);
            department.IsDeleted = true;

            try
            {
               await _unitOfWork.Departments.UpdateAsync(department);
               await _unitOfWork.SaveChangesAsync();
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

        public async Task<DepartmentsResponseModel> GetAllDepartment()
        {
            var response = new DepartmentsResponseModel();

            try
            {
                Expression<Func<Department, bool>> expression = c => c.IsDeleted == false;
                var department = await _unitOfWork.Departments.GetAllAsync(expression);

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

        public async Task<DepartmentResponseModel> GetDepartment(string departmentId)
        {
            var response = new DepartmentResponseModel();

            Expression<Func<Department, bool>> expression = c =>
                                                (c.Id == departmentId)
                                                && (c.Id == departmentId
                                                && c.IsDeleted == false);

            var departmentExist = await _unitOfWork.Departments.ExistsAsync(expression);

            if (!departmentExist)
            {
                response.Message = $"Department with id {departmentId} does not exist.";
                return response;
            }

            var department = await _unitOfWork.Departments.GetAsync(departmentId);

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

        public async Task<IEnumerable<SelectListItem>> SelectDepartment()
        {
            var departments = await _unitOfWork.Departments.SelectAll();

            return  departments.Select(dept => new SelectListItem()
            {
                Text = dept.Name,
                Value = dept.Id
            });
        }

        public async Task<BaseResponseModel> UpdateDepartment(string departmentId, UpdateDepartmentViewModel request)
        {
            var response = new BaseResponseModel();
            string modifiedBy = _httpContextAccessor.HttpContext.User.Identity.Name;
            var departmentExist = await  _unitOfWork.Departments.ExistsAsync(c => c.Id == departmentId);

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

            var department = await _unitOfWork.Departments.GetAsync(departmentId);
            department.Name = request.Name; 
            department.Description = request.Description;
            department.ModifiedBy = modifiedBy;
            
            
            try 
            { 
               await _unitOfWork.Departments.UpdateAsync(department);
                await _unitOfWork.SaveChangesAsync();
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

