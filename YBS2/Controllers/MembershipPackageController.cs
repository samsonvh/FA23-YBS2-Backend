using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using YBS2.Data.Enums;
using YBS2.Middlewares.AuthenticationFilter;
using YBS2.Service.Dtos.Details;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;
using YBS2.Service.Dtos.PageResponses;
using YBS2.Service.Services;
using YBS2.Service.Utils;

namespace YBS2.Controllers
{
    [Route(APIEndPoints.MEMBERSHIP_PACKAGES_V1)]
    [ApiController]
    public class MembershipPackageController : ControllerBase
    {
        private readonly ILogger<MembershipPackageController> _logger;
        private readonly IMembershipPackageService _membershipPackageService;
        private readonly IConfiguration _configuration;

        public MembershipPackageController(ILogger<MembershipPackageController> logger, IMembershipPackageService membershipPackageService, IConfiguration configuration)
        {
            _logger = logger;
            _membershipPackageService = membershipPackageService;
            _configuration = configuration;
        }
        [SwaggerOperation("[Public] Get list of membership packages, paging information")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(DefaultPageResponse<MembershipPackageListingDto>))]
        [Produces("application/json")]
        [HttpGet]
        
        public async Task<IActionResult> GetAll([FromQuery] MembershipPackagePageRequest pageRequest)
        {
            ClaimsPrincipal claims = JWTUtils.GetClaim(_configuration,Request.Headers["Authorization"]);
            return Ok(await _membershipPackageService.GetAll(pageRequest,claims));
        }

        [SwaggerOperation("[Public] Get details of a membership package according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(MembershipPackageDto))]
        [Produces("application/json")]
        [Route(APIEndPoints.MEMBERSHIP_PACKAGES_ID_V1)]
        [HttpGet]
        [RoleAuthorization(nameof(EnumRole.Admin))]
        public async Task<IActionResult> GetDetails([FromRoute] Guid id)
        {
            ClaimsPrincipal claims = JWTUtils.GetClaim(_configuration,Request.Headers["Authorization"]);
            return Ok(await _membershipPackageService.GetDetails(id,claims));
        }

        [SwaggerOperation("[Admin] Create new membership package")]
        [SwaggerResponse(StatusCodes.Status201Created, "Success", typeof(MembershipPackageDto))]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] MembershipPackageInputDto inputDto)
        {
            return CreatedAtAction(nameof(Create) ,await _membershipPackageService.Create(inputDto));
        }

        [SwaggerOperation("[Admin] Update membership package details according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(MembershipPackageDto))]
        [Produces("application/json")]
        [Route(APIEndPoints.MEMBERSHIP_PACKAGES_ID_V1)]
        [HttpPut]
        [RoleAuthorization(nameof(EnumRole.Member))]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] MembershipPackageInputDto inputDto)
        {
            return Ok(await _membershipPackageService.Update(id, inputDto));
        }

        [SwaggerOperation("[Admin] Change status of membership package according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(bool))]
        [Produces("application/json")]
        [Route(APIEndPoints.MEMBERSHIP_PACKAGES_ID_V1)]
        [HttpPatch]
        [RoleAuthorization(nameof(EnumRole.Admin))]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromBody] string status)
        {
            return Ok(await _membershipPackageService.ChangeStatus(id, status));
        }
    }
}