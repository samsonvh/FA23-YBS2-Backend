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
    public interface IBookingService : IDefaultService<BookingPageRequest,BookingListingDto,BookingDto,BookingInputDto>
    {
        Task<DefaultPageResponse<BookingListingDto>> GetAll(BookingPageRequest pageRequest, ClaimsPrincipal claims);
        Task<BookingDto?> GetDetails (Guid id, ClaimsPrincipal claims);
        Task<BookingDto> Create (BookingInputDto inputDto, ClaimsPrincipal claims);
    }
}