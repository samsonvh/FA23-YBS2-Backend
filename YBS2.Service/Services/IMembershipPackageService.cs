using YBS2.Service.Dtos.Details;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;

namespace YBS2.Service.Services
{
    public interface IMembershipPackageService : IDefaultService<MembershipPackagePageRequest, MembershipPackageListingDto, MembershipPackageDto, MembershipPackageInputDto>
    {

    }
}