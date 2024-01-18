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
using YBS2.Service.Services.Implements;
using YBS2.Service.Utils;

namespace YBS2.Controllers
{
    [ApiController]
    public class PassengersController : ControllerBase
    {
        private readonly ILogger<PassengersController> _logger;
        private readonly IPassengerService _passengerService;
        private readonly IConfiguration _configuration;

        public PassengersController(ILogger<PassengersController> logger, PassengerService passengerService, IConfiguration configuration)
        {
            _logger = logger;
            _passengerService = passengerService;
            _configuration = configuration;
        }
        [SwaggerOperation("Get list of passengers, paging information")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(DefaultPageResponse<PassengerListingDto>))]
        [Produces("application/json")]
        [HttpGet]
        [Route(APIEndPoints.PASSENGER_OF_BOOKING_V1)]
        public async Task<IActionResult> GetAllOfBooking([FromRoute] Guid id, [FromQuery] PassengerPageRequest pageRequest)
        {
            ClaimsPrincipal claims = JWTUtils.GetClaim(_configuration, Request.Headers["Authorization"]);
            return Ok(await _passengerService.GetAllOfBooking(id, pageRequest ,claims));
        }

        
    }
}