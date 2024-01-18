using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YBS2.Service.Dtos.Details;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;
using YBS2.Service.Dtos.PageResponses;

namespace YBS2.Service.Services
{
    public interface ITourActivityService : IDefaultService<TourActivityPageRequest,TourActivityListingDto,TourActivityDto,TourActivityInputDto>
    {
        Task<DefaultPageResponse<TourActivityListingDto>> GetAllOfTour(Guid id, TourActivityPageRequest pageRequest, ClaimsPrincipal claims);
        Task<TourActivityDto> GetDetails(Guid id, Guid tourActivityId, ClaimsPrincipal claims);
        Task<List<TourActivityDto>> CreateOfTour (List<TourActivityInputDto> inputDtos, Guid id, ClaimsPrincipal claims);
    }
}