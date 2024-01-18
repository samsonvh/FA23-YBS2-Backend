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
using YBS2.Middlewares.AuthenticationFilter;
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
    public class BookingsController : ControllerBase
    {
        private readonly ILogger<BookingsController> _logger;
        private readonly IBookingService _bookingService;
        private readonly IConfiguration _configuration;

        public BookingsController(ILogger<BookingsController> logger, IBookingService bookingService, IConfiguration configuration)
        {
            _logger = logger;
            _bookingService = bookingService;
            _configuration = configuration;
        }
        [SwaggerOperation("[Company|Member] Get list of bookings, paging information")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(DefaultPageResponse<BookingListingDto>))]
        [Produces("application/json")]
        [HttpGet]
        [RoleAuthorization($"{nameof(EnumRole.Company)},{nameof(EnumRole.Member)}")]
        public async Task<IActionResult> GetAll([FromQuery] BookingPageRequest pageRequest)
        {
            ClaimsPrincipal claims = JWTUtils.GetClaim(_configuration, Request.Headers["Authorization"]);
            return Ok(await _bookingService.GetAll(pageRequest, claims));
        }

        [SwaggerOperation("[Public] Get details of a booking according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(BookingDto))]
        [Produces("application/json")]
        [HttpGet]
        [Route(APIEndPoints.BOOKING_ID_V1)]
        public async Task<IActionResult> GetDetails([FromRoute] Guid id)
        {
            ClaimsPrincipal claims = JWTUtils.GetClaim(_configuration, Request.Headers["Authorization"]);
            return Ok(await _bookingService.GetDetails(id, claims));
        }

        [SwaggerOperation("[Public] Create new booking")]
        [SwaggerResponse(StatusCodes.Status201Created, "Success", typeof(BookingDto))]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookingInputDto inputDto)
        {
            ClaimsPrincipal claims = JWTUtils.GetClaim(_configuration, Request.Headers["Authorization"]);
            return CreatedAtAction(nameof(Create), await _bookingService.Create(inputDto, claims, HttpContext));
        }



        [SwaggerOperation("[Company] Change status of booking according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(bool))]
        [Produces("application/json")]
        [Route(APIEndPoints.BOOKING_ID_V1)]
        [HttpPatch]
        [RoleAuthorization($"{nameof(EnumRole.Company)}")]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromBody] string status)
        {
            ClaimsPrincipal claims = JWTUtils.GetClaim(_configuration, Request.Headers["Authorization"]);
            return Ok(await _bookingService.ChangeStatus(id, status, claims));

        }
        [SwaggerOperation("[Public] System confirm booking after user make an payment")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(bool))]
        [Produces("application/json")]
        [Route(APIEndPoints.BOOKING_CONFIRM)]
        [HttpGet]
        public async Task<IActionResult> ConfirmBooking()
        {
            return Ok(await _bookingService.ConfirmBooking(Request.Query));
        }

        [SwaggerOperation("[Public] Create Booking Payment URL when member pay for Not Yet paid booking")]
        [SwaggerResponse(StatusCodes.Status201Created, "Success", typeof(MemberDto))]
        [Produces("application/json")]
        [Route(APIEndPoints.BOOKING_CREATE_PAYMENT_URL)]
        [HttpPost]
        [RoleAuthorization($"{nameof(EnumRole.Member)}")]
        public async Task<IActionResult> CreateBookingPaymentURL([FromRoute] Guid id)
        {
            return CreatedAtAction(nameof(CreateBookingPaymentURL), await _bookingService.CreateBookingPaymentURL(id, HttpContext));
        }
    }
}