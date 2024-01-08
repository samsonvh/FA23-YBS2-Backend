using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
    [Route(APIEndPoints.DOCK_V1)]
    [RoleAuthorization(nameof(EnumRole.Company))]
    public class DockController : ControllerBase
    {
        private readonly ILogger<DockController> _logger;
        private readonly IDockService _dockService;
        private readonly IConfiguration _configuration;

        public DockController(ILogger<DockController> logger, IDockService dockService, IConfiguration configuration)
        {
            _logger = logger;
            _dockService = dockService;
            _configuration = configuration;
        }
        [SwaggerOperation("[Company] Get list of docks, paging information")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(DefaultPageResponse<DockListingDto>))]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] DockPageRequest pageRequest)
        {
            ClaimsPrincipal claims = JWTUtils.GetClaim(_configuration, Request.Headers["Authorization"]);
            return Ok(await _dockService.GetAll(pageRequest, claims));
        }

        [SwaggerOperation("[Company] Get details of a dock according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(DockDto))]
        [Produces("application/json")]
        [HttpGet]
        [Route(APIEndPoints.DOCK_ID_V1)]
        public async Task<IActionResult> GetDetails([FromRoute] Guid id)
        {
            ClaimsPrincipal claims = JWTUtils.GetClaim(_configuration, Request.Headers["Authorization"]);
            return Ok(await _dockService.GetDetails(id, claims));
        }

        [SwaggerOperation("[Company] Create new dock")]
        [SwaggerResponse(StatusCodes.Status201Created, "Success", typeof(DockDto))]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] DockInputDto inputDto)
        {
            ClaimsPrincipal claims = JWTUtils.GetClaim(_configuration, Request.Headers["Authorization"]);
            return CreatedAtAction(nameof(Create) ,await _dockService.Create(inputDto, claims));
        }

        [SwaggerOperation("[Company] Update dock details according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(DockDto))]
        [Produces("application/json")]
        [HttpPut]
        [Route(APIEndPoints.DOCK_ID_V1)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] DockInputDto inputDto)
        {
            return Ok(await _dockService.Update(id, inputDto));
        }

        [SwaggerOperation("[Company] Change status of dock according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(bool))]
        [Produces("application/json")]
        [Route(APIEndPoints.DOCK_ID_V1)]
        [HttpPatch]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromBody] string status)
        {
            ClaimsPrincipal claims = JWTUtils.GetClaim(_configuration, Request.Headers["Authorization"]);
            return Ok(await _dockService.ChangeStatus(id, status, claims));

        }
    }
}