using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YBS2.Service.Dtos;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;

namespace YBS2.Service.Services
{
    public interface IMemberService : IDefaultService<MemberPageRequest,MemberListingDto,MemberDto,MemberInputDto>
    {
        Task<bool> ActivateMember(ActivateMemberInputDto inputDto);
        Task<MemberDto> Update (MemberInputDto inputDto);
    }
}