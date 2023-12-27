using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

namespace YBS2.Controllers
{
    [ApiController]
    [Route(APIEndPoints.YACHT_V1)]
    [RoleAuthorization(nameof(EnumRole.Company))]
    public class YachtController : ControllerBase
    {
        private readonly ILogger<YachtController> _logger;
        private readonly IYachtService _yachtService;

        public YachtController(ILogger<YachtController> logger, IYachtService yachtService)
        {
            _logger = logger;
            _yachtService = yachtService;
        }
        [SwaggerOperation("Get list of yachts, paging information")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(DefaultPageResponse<YachtListingDto>))]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] YachtPageRequest pageRequest)
        {
            return Ok(await _yachtService.GetAll(pageRequest));
        }

        [SwaggerOperation("Get details of a yacht according to ID")]
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

        [SwaggerOperation("Create new yacht")]
        [SwaggerResponse(StatusCodes.Status201Created, "Success", typeof(YachtDto))]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] YachtInputDto inputDto)
        {
            return Ok(await _yachtService.Create(inputDto));
        }

        [SwaggerOperation("Update yacht details according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(YachtDto))]
        [Produces("application/json")]
        [HttpPut]
        [Route(APIEndPoints.YACHT_ID_V1)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] YachtInputDto inputDto)
        {
            return Ok(await _yachtService.Update(id, inputDto));
        }

        [SwaggerOperation("Change status of yacht according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(bool))]
        [Produces("application/json")]
        [Route(APIEndPoints.YACHT_ID_V1)]
        [HttpPatch]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromBody] string status)
        {
            return Ok(await _yachtService.ChangeStatus(id, status));

        }
    }
}