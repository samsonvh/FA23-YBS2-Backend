using YBS2.Data.Enums;

namespace YBS2.Service.Dtos.PageRequests
{
    public class UpdateRequestPageRequest : DefaultPageRequest
    {
        public string? CompanyName { get; set; }
        public EnumUpdateRequestStatus? Status { get; set; }
    }
}
