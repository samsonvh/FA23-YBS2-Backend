using YBS2.Data.Enums;

namespace YBS2.Service.Dtos.PageRequests
{
    public class MembershipPackagePageRequest : DefaultPageRequest
    {
        public string? Name { get; set; }
        public float? MinPrice { get; set; }
        public float? MaxPrice { get; set; }
        public EnumMembershipPackageStatus? Status { get; set; }
    }
}
