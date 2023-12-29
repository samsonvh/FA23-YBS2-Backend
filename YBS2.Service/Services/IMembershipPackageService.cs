using System.Security.Claims;
using YBS2.Service.Dtos.Details;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;
using YBS2.Service.Dtos.PageResponses;

namespace YBS2.Service.Services
{
    public interface IMembershipPackageService : IDefaultService<MembershipPackagePageRequest, MembershipPackageListingDto, MembershipPackageDto, MembershipPackageInputDto>
    {
        Task<DefaultPageResponse<MembershipPackageListingDto>> GetAll (MembershipPackagePageRequest pageRequest, ClaimsPrincipal claims);
        Task<MembershipPackageDto?> GetDetails (Guid id, ClaimsPrincipal claims);
    }
}