namespace ComplaintRequestSystem.Models.Complaint
{
    public class ComplaintResponseModel : BaseResponseModel
    {
        public ComplaintViewModel Data { get; set; }
    }

    public class ComplaintsResponseModel : BaseResponseModel
    {
        public List<ComplaintViewModel> Data { get; set; }
    }
}
