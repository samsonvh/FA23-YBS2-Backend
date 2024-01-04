using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YBS2.Service.Dtos;
using YBS2.Service.Dtos.Details;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;
using YBS2.Service.Dtos.PageResponses;

namespace YBS2.Service.Services
{
    public interface ITourService : IDefaultService<TourPageRequest, TourListingDto, TourDto, TourInputDto>
    {
        Task<TourDto?> Create(TourInputDto inputDto, ClaimsPrincipal claims);
        Task<DefaultPageResponse<TourListingDto>> GetAll(TourPageRequest pageRequest, ClaimsPrincipal claims);
        Task<TourDto?> GetDetails(Guid id, ClaimsPrincipal claims);
        Task<bool> ChangeStatus(Guid id, string status, ClaimsPrincipal claims);
    }
}