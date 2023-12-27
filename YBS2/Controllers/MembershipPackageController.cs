using Microsoft.AspNetCore.Mvc;
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
    [Route(APIEndPoints.MEMBERSHIP_PACKAGES_V1)]
    [ApiController]
    public class MembershipPackageController : ControllerBase
    {
        private readonly ILogger<MembershipPackageController> _logger;
        private readonly IMembershipPackageService _membershipPackageService;

        public MembershipPackageController(ILogger<MembershipPackageController> logger, IMembershipPackageService membershipPackageService)
        {
            _logger = logger;
            _membershipPackageService = membershipPackageService;
        }
        [SwaggerOperation("Get list of membership packages, paging information")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(DefaultPageResponse<MembershipPackageListingDto>))]
        [Produces("application/json")]
        [HttpGet]
        
        public async Task<IActionResult> GetAll([FromQuery] MembershipPackagePageRequest pageRequest)
        {
            return Ok(await _membershipPackageService.GetAll(pageRequest));
        }

        [SwaggerOperation("Get details of a membership package according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(MembershipPackageDto))]
        [Produces("application/json")]
        [Route(APIEndPoints.MEMBERSHIP_PACKAGES_ID_V1)]
        [HttpGet]
        [RoleAuthorization(nameof(EnumRole.Admin))]
        public async Task<IActionResult> GetDetails([FromRoute] Guid id)
        {
            return Ok(await _membershipPackageService.GetDetails(id));
        }

        [SwaggerOperation("Create new membership package")]
        [SwaggerResponse(StatusCodes.Status201Created, "Success", typeof(MembershipPackageDto))]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] MembershipPackageInputDto inputDto)
        {
            return Ok(await _membershipPackageService.Create(inputDto));
        }

        [SwaggerOperation("Update membership package details according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(MembershipPackageDto))]
        [Produces("application/json")]
        [Route(APIEndPoints.MEMBERSHIP_PACKAGES_ID_V1)]
        [HttpPut]
        [RoleAuthorization(nameof(EnumRole.Member))]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] MembershipPackageInputDto inputDto)
        {
            return Ok(await _membershipPackageService.Update(id, inputDto));
        }

        [SwaggerOperation("Change status of membership package according to ID")]
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