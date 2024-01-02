namespace YBS2.Service.Dtos.PageRequests
{
    public class DefaultPageRequest
    {
        public string OrderBy { get; set; } = "Id";
        public bool IsDescending { get; set; } = false;
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 15;
    }
}