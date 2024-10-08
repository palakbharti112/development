namespace Job_PortalApi.Models
{
    public class GeneralResponseModel
    {
        public object Data { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }
}
