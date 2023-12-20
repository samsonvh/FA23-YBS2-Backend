namespace YBS2.Service.Dtos.PageResponses
{
    public class DefaultPageResponse<L>
    {
        public List<L>? Data { get; set; }
        public int TotalItem { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
    }
}