namespace YBS2.Service.Dtos.PageRequests
{
    public class CompanyPageRequest : DefaultPageRequest
    {
        public string? Name { get; set; }
        public string? HotLine { get; set; }
    }
}
