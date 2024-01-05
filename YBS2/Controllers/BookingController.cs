using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using YBS2.Data.Enums;
using YBS2.Service.Dtos;
using YBS2.Service.Dtos.Details;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;
using YBS2.Service.Dtos.PageResponses;
using YBS2.Service.Services;
using YBS2.Service.Utils;

namespace YBS2.Controllers
{
    [ApiController]
    [Route(APIEndPoints.BOOKING_V1)]
    // [RoleAuthorization(nameof(EnumRole.Company))]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingService _bookingService;
        private readonly IConfiguration _configuration;

        public BookingController(ILogger<BookingController> logger, IBookingService bookingService, IConfiguration configuration)
        {
            _logger = logger;
            _bookingService = bookingService;
            _configuration = configuration;
        }
        [SwaggerOperation("[Company|Member] Get list of bookings, paging information")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(DefaultPageResponse<BookingListingDto>))]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] BookingPageRequest pageRequest)
        {
            ClaimsPrincipal claims = JWTUtils.GetClaim(_configuration,Request.Headers["Authorization"]);
            return Ok(await _bookingService.GetAll(pageRequest, claims));
        }

        [SwaggerOperation("[Public] Get details of a booking according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(BookingDto))]
        [Produces("application/json")]
        [HttpGet]
        [Route(APIEndPoints.BOOKING_ID_V1)]
        public async Task<IActionResult> GetDetails([FromRoute] Guid id)
        {
            ClaimsPrincipal claims = JWTUtils.GetClaim(_configuration,Request.Headers["Authorization"]);
            return Ok(await _bookingService.GetDetails(id,claims));
        }

        [SwaggerOperation("[Public] Create new booking")]
        [SwaggerResponse(StatusCodes.Status201Created, "Success", typeof(BookingDto))]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookingInputDto inputDto)
        {
            ClaimsPrincipal claims = JWTUtils.GetClaim(_configuration,Request.Headers["Authorization"]);
            return Ok(await _bookingService.Create(inputDto, claims, HttpContext));
        }

        [SwaggerOperation("[Company|Member] Update booking details according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(BookingDto))]
        [Produces("application/json")]
        [HttpPut]
        [Route(APIEndPoints.BOOKING_ID_V1)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] BookingInputDto inputDto)
        {
            return Ok(await _bookingService.Update(id, inputDto));
        }

        [SwaggerOperation("[Company] Change status of booking according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(bool))]
        [Produces("application/json")]
        [Route(APIEndPoints.BOOKING_ID_V1)]
        [HttpPatch]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromBody] string status)
        {
            return Ok(await _bookingService.ChangeStatus(id, status));

        }
        [SwaggerOperation("[Public] System confirm booking after user make an payment")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(bool))]
        [Produces("application/json")]
        [Route(APIEndPoints.BOOKING_CONFIRM)]
        [HttpGet]
        public async Task<IActionResult> ActivateMember()
        {
            return Ok(await _bookingService.ConfirmBooking(Request.Query));
        }
    }
}