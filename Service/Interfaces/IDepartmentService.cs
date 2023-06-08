using ComplaintRequestSystem.Models.Department;
using ComplaintRequestSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ComplaintRequestSystem.Service.Interfaces
{
    public interface IDepartmentService
    {
        BaseResponseModel CreateDepartment(CreateDepartmentViewModel request);
        BaseResponseModel DeleteDepartment(string departmentId);
        BaseResponseModel UpdateDepartment(string departmentId, UpdateDepartmentViewModel request);
        DepartmentResponseModel GetDepartment(string departmentId);
        DepartmentsResponseModel GetAllDepartment();
        IEnumerable<SelectListItem> SelectDepartment();
    }
}
