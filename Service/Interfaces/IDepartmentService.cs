using ComplaintRequestSystem.Models.Department;
using ComplaintRequestSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ComplaintRequestSystem.Service.Interfaces
{
    public interface IDepartmentService
    {
        Task<BaseResponseModel> CreateDepartment(CreateDepartmentViewModel request);
        Task<BaseResponseModel> DeleteDepartment(string departmentId);
        Task<BaseResponseModel> UpdateDepartment(string departmentId, UpdateDepartmentViewModel request);
        Task<DepartmentResponseModel> GetDepartment(string departmentId);
        Task<DepartmentsResponseModel> GetAllDepartment();
        Task<IEnumerable<SelectListItem>> SelectDepartment();
    }
}
