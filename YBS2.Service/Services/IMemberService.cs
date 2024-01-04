using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using YBS2.Service.Dtos;
using YBS2.Service.Dtos.Details;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;

namespace YBS2.Service.Services
{
    public interface IMemberService : IDefaultService<MemberPageRequest, MemberListingDto, MemberDto, MemberInputDto>
    {
        Task<MemberDto> ActivateMember(IQueryCollection collections);
        Task<MemberDto> Update(MemberInputDto inputDto, ClaimsPrincipal claims);
        Task<string> Create(MemberInputDto inputDto, HttpContext context);
    }
}