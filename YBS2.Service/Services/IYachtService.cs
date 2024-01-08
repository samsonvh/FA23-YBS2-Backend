using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YBS2.Service.Dtos;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;
using YBS2.Service.Dtos.PageResponses;

namespace YBS2.Service.Services
{
    public interface IYachtService : IDefaultService<YachtPageRequest,YachtListingDto,YachtDto,YachtInputDto>
    {
        Task<DefaultPageResponse<YachtListingDto>> GetAll(YachtPageRequest pageRequest, ClaimsPrincipal claims);
        Task<YachtDto?> GetDetails(Guid id, ClaimsPrincipal claims);
    }
}