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
    [Route(APIEndPoints.ACTIVITY_OF_TOUR_V1)]
    [Consumes("application/json")]

    public class TourActivitiesController : ControllerBase
    {
        private readonly ILogger<TourActivitiesController> _logger;
        private readonly ITourActivityService _tourActivityService;
        private readonly IConfiguration _configuration;

        public TourActivitiesController(ILogger<TourActivitiesController> logger, ITourActivityService tourActivityService, IConfiguration configuration)
        {
            _logger = logger;
            _tourActivityService = tourActivityService;
            _configuration = configuration;
        }
        [SwaggerOperation("[Public] Get list of activities of tour, paging information")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(DefaultPageResponse<TourActivityListingDto>))]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetAllOfTour([FromRoute] Guid id, [FromQuery] TourActivityPageRequest pageRequest)
        {
            ClaimsPrincipal claims = JWTUtils.GetClaim(_configuration, Request.Headers["Authorization"]);
            return Ok(await _tourActivityService.GetAllOfTour(id, pageRequest, claims));
        }

        [SwaggerOperation("[Public] Get details of a activity of tour according to activityId")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(TourActivityDto))]
        [Produces("application/json")]
        [HttpGet]
        [Route(APIEndPoints.ACTIVITY_OF_TOUR_ID_V1)]

        public async Task<IActionResult> GetDetails([FromRoute] Guid id, [FromRoute] Guid tourActivityId)
        {
            ClaimsPrincipal claims = JWTUtils.GetClaim(_configuration, Request.Headers["Authorization"]);
            return Ok(await _tourActivityService.GetDetails(id, tourActivityId, claims));
        }
 
        [SwaggerOperation("[Company] Create new activity")]
        [SwaggerResponse(StatusCodes.Status201Created, "Success", typeof(TourActivityDto))]
        [Produces("application/json")]
        [HttpPost]
        [RoleAuthorization(nameof(EnumRole.Company))]
        [Route(APIEndPoints.ACTIVITY_OF_TOUR_V1)]
        public async Task<IActionResult> CreateOfTour([FromBody] List<TourActivityInputDto> inputDtos, [FromRoute] Guid id)
        {
            ClaimsPrincipal claims = JWTUtils.GetClaim(_configuration, Request.Headers["Authorization"]);
            return Ok(await _tourActivityService.CreateOfTour(inputDtos,id, claims));   
        }

        [SwaggerOperation("[Company] Update activity details according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(TourActivityDto))]
        [Produces("application/json")]
        [HttpPut]
        [RoleAuthorization(nameof(EnumRole.Company))]
        public async Task<IActionResult> UpdateOfTour([FromRoute] Guid id, [FromForm] TourActivityInputDto inputDto)
        {
            return Ok();
        }

        [SwaggerOperation("[Company] Change status of activity according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(bool))]
        [Produces("application/json")]
        [Route(APIEndPoints.ACTIVITY_ID_V1)]
        [HttpPatch]
        [RoleAuthorization(nameof(EnumRole.Company))]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromBody] string status)
        {
            return Ok();
        }
    }
}