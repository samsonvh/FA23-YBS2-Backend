using YBS2.Data.Enums;

namespace YBS2.Service.Dtos.PageRequests
{
    public class CompanyPageRequest : DefaultPageRequest
    {
        public string? Name { get; set; }
        public EnumAccountStatus? Status { get; set; }
    }
}
