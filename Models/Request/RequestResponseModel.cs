namespace ComplaintRequestSystem.Models.Request
{
    public class RequestResponseModel : BaseResponseModel
    {
        public RequestViewModel Data { get; set; }
    }

    public class RequestsResponseModel : BaseResponseModel
    {
        public List<RequestViewModel> Data { get; set; }
    }
}
