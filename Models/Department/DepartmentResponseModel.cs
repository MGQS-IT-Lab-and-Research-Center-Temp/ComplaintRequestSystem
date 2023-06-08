namespace ComplaintRequestSystem.Models.Department
{
    public class DepartmentResponseModel : BaseResponseModel
    {
        public DepartmentViewModel Data { get; set; }
    }

    public class DepartmentsResponseModel : BaseResponseModel
    {
        public List<DepartmentViewModel> Data { get; set; }
    }
}
