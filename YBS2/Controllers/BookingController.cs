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
using YBS.Middlewares;
using YBS2.Data.Enums;
using YBS2.Service.Dtos;
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
        [SwaggerOperation("Get list of bookings, paging information")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(DefaultPageResponse<BookingListingDto>))]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] BookingPageRequest pageRequest)
        {
            return Ok(await _bookingService.GetAll(pageRequest));
        }

        [SwaggerOperation("Get details of a booking according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(BookingDto))]
        [Produces("application/json")]
        [HttpGet]
        [Route(APIEndPoints.BOOKING_ID_V1)]
        public async Task<IActionResult> GetDetails([FromRoute] Guid id)
        {
            return Ok(await _bookingService.GetDetails(id));
        }

        [SwaggerOperation("Create new booking")]
        [SwaggerResponse(StatusCodes.Status201Created, "Success", typeof(BookingDto))]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BookingInputDto inputDto)
        {
            return Ok(await _bookingService.Create(inputDto));
        }

        [SwaggerOperation("Update booking details according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(BookingDto))]
        [Produces("application/json")]
        [HttpPut]
        [Route(APIEndPoints.BOOKING_ID_V1)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] BookingInputDto inputDto)
        {
            return Ok(await _bookingService.Update(id, inputDto));
        }

        [SwaggerOperation("Change status of booking according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(bool))]
        [Produces("application/json")]
        [Route(APIEndPoints.BOOKING_ID_V1)]
        [HttpPatch]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromBody] string status)
        {
            return Ok(await _bookingService.ChangeStatus(id, status));

        }
    }
}