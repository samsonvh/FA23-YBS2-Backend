namespace YBS2.Service.Dtos.PageRequests
{
    public class DefaultPageRequest
    {
        public string? OrderBy { get; set; } = null;
        public bool IsAscending { get; set; } = true;
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}