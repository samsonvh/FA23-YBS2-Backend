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
    public interface IPassengerService : IDefaultService<PassengerPageRequest, PassengerListingDto, PassengerDto, PassengerInputDto>
    {
        Task<DefaultPageResponse<PassengerListingDto>> GetAllOfBooking(Guid bookingId, PassengerPageRequest pageRequest, ClaimsPrincipal claims);
    }
}