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
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;
using YBS2.Service.Dtos.PageResponses;
using YBS2.Service.Services;
using YBS2.Service.Utils;

namespace YBS2.Controllers
{
    [ApiController]
    [Route(APIEndPoints.YACHT_V1)]
    public class YachtController : ControllerBase
    {
        private readonly ILogger<YachtController> _logger;
        private readonly IYachtService _yachtService;
        private readonly IConfiguration _configuration;

        public YachtController(ILogger<YachtController> logger, IYachtService yachtService, IConfiguration configuration)
        {
            _logger = logger;
            _yachtService = yachtService;
            _configuration = configuration;
        }
        [SwaggerOperation("[Company] Get list of yachts, paging information")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(DefaultPageResponse<YachtListingDto>))]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] YachtPageRequest pageRequest)
        {
            ClaimsPrincipal claims = JWTUtils.GetClaim(_configuration, Request.Headers["Authorization"]);
            return Ok(await _yachtService.GetAll(pageRequest, claims));
        }

        [SwaggerOperation("[Company] Get details of a yacht according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(YachtDto))]
        [Produces("application/json")]
        [HttpGet]
        [Route(APIEndPoints.YACHT_ID_V1)]
        public async Task<IActionResult> GetDetails([FromRoute] Guid id)
        {
            YachtDto? yachtshipPackageDto = await _yachtService.GetDetails(id);
            if (yachtshipPackageDto != null)
            {
                return Ok(yachtshipPackageDto);
            }
            else
            {
                return Ok();
            }
        }

        [SwaggerOperation("[Company] Create new yacht")]
        [SwaggerResponse(StatusCodes.Status201Created, "Success", typeof(YachtDto))]
        [Produces("application/json")]
        [HttpPost]
        [RoleAuthorization(nameof(EnumRole.Company))]
        public async Task<IActionResult> Create([FromForm] YachtInputDto inputDto)
        {
            return CreatedAtAction(nameof(Create), await _yachtService.Create(inputDto));
        }

        [SwaggerOperation("[Company] Update yacht details according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(YachtDto))]
        [Produces("application/json")]
        [HttpPut]
        [Route(APIEndPoints.YACHT_ID_V1)]
        [RoleAuthorization(nameof(EnumRole.Company))]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] YachtInputDto inputDto)
        {
            return Ok(await _yachtService.Update(id, inputDto));
        }

        [SwaggerOperation("[Company] Change status of yacht according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(bool))]
        [Produces("application/json")]
        [Route(APIEndPoints.YACHT_ID_V1)]
        [HttpPatch]
        [RoleAuthorization(nameof(EnumRole.Company))]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromBody] string status)
        {
            return Ok(await _yachtService.ChangeStatus(id, status));

        }
    }
}