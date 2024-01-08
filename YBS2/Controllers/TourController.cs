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
    [Route(APIEndPoints.TOUR_V1)]
    
    public class TourController : ControllerBase
    {
        private readonly ILogger<TourController> _logger;
        private readonly ITourService _tourService;
        private readonly IConfiguration _configuration;

        public TourController(ILogger<TourController> logger, ITourService tourService, IConfiguration configuration)
        {
            _logger = logger;
            _tourService = tourService;
            _configuration = configuration;
        }
        [SwaggerOperation("[Public] Get list of tours, paging information")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(DefaultPageResponse<TourListingDto>))]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] TourPageRequest pageRequest)
        {
            ClaimsPrincipal claims = JWTUtils.GetClaim(_configuration, Request.Headers["Authorization"]);
            return Ok(await _tourService.GetAll(pageRequest, claims));
        }

        [SwaggerOperation("[Public] Get details of a tour according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(TourDto))]
        [Produces("application/json")]
        [HttpGet]
        [Route(APIEndPoints.TOUR_ID_V1)]
        
        public async Task<IActionResult> GetDetails([FromRoute] Guid id)
        {
            ClaimsPrincipal claims = JWTUtils.GetClaim(_configuration, Request.Headers["Authorization"]);
            return Ok(await _tourService.GetDetails(id, claims));
        }

        [SwaggerOperation("[Company] Create new tour")]
        [SwaggerResponse(StatusCodes.Status201Created, "Success", typeof(TourDto))]
        [Produces("application/json")]
        [HttpPost]
        [RoleAuthorization(nameof(EnumRole.Company))]
        public async Task<IActionResult> Create([FromForm] TourInputDto inputDto)
        {
            ClaimsPrincipal claims = JWTUtils.GetClaim(_configuration, Request.Headers["Authorization"]);
            return CreatedAtAction(nameof(Create) ,await _tourService.Create(inputDto,claims));
        }

        [SwaggerOperation("[Company] Update tour details according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(TourDto))]
        [Produces("application/json")]
        [HttpPut]
        [Route(APIEndPoints.TOUR_ID_V1)]
        [RoleAuthorization(nameof(EnumRole.Company))]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] TourInputDto inputDto)
        {
            return Ok(await _tourService.Update(id, inputDto));
        }

        [SwaggerOperation("[Company] Change status of tour according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(bool))]
        [Produces("application/json")]
        [Route(APIEndPoints.TOUR_ID_V1)]
        [HttpPatch]
        [RoleAuthorization(nameof(EnumRole.Company))]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromBody] string status)
        {
            ClaimsPrincipal claims = JWTUtils.GetClaim(_configuration, Request.Headers["Authorization"]);
            return Ok(await _tourService.ChangeStatus(id, status, claims));

        }
    }
}