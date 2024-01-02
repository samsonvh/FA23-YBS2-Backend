using System.Security.Claims;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using YBS2.Middlewares.AuthenticationFilter;
using YBS2.Data.Enums;
using YBS2.Service.Dtos.Details;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;
using YBS2.Service.Dtos.PageResponses;
using YBS2.Service.Exceptions;
using YBS2.Service.Services;
using YBS2.Service.Utils;

namespace YBS2.Controllers
{
    [Route(APIEndPoints.MEMBER_V1)]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly ILogger<MemberController> _logger;
        private readonly IMemberService _memberService;
        private readonly IConfiguration _configuration;

        public MemberController(ILogger<MemberController> logger, IMemberService memberService, IConfiguration configuration)
        {
            _logger = logger;
            _memberService = memberService;
            _configuration = configuration;
        }

        [SwaggerOperation("Get list of members, paging information")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(DefaultPageResponse<MemberListingDto>))]
        [Produces("application/json")]
        [HttpGet]
        [RoleAuthorization(nameof(EnumRole.Admin))]
        public async Task<IActionResult> GetAll([FromQuery] MemberPageRequest pageRequest)
        {
            return Ok(await _memberService.GetAll(pageRequest));
        }

        [SwaggerOperation("Get details of a member according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(MemberDto))]
        [Produces("application/json")]
        [Route(APIEndPoints.MEMBER_ID_V1)]
        [HttpGet]
        [RoleAuthorization(nameof(EnumRole.Admin))]
        public async Task<IActionResult> GetDetails([FromRoute] Guid id)
        {
            return Ok(await _memberService.GetDetails(id));
        }

        [SwaggerOperation("Create new member")]
        [SwaggerResponse(StatusCodes.Status201Created, "Success", typeof(MemberDto))]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] MemberInputDto inputDto)
        {
            return Ok(await _memberService.Create(inputDto));
        }

        [SwaggerOperation("Update member details according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(MemberDto))]
        [Produces("application/json")]
        [HttpPut]
        [RoleAuthorization(nameof(EnumRole.Member))]
        public async Task<IActionResult> Update([FromForm] MemberInputDto inputDto)
        {
            ClaimsPrincipal claims = JWTUtils.GetClaim(_configuration, Request.Headers["Authorization"]);
            return Ok(await _memberService.Update(inputDto, claims));
        }

        [SwaggerOperation("Change status of member according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(bool))]
        [Produces("application/json")]
        [Route(APIEndPoints.MEMBER_ID_V1)]
        [HttpPatch]
        [RoleAuthorization(nameof(EnumRole.Admin))]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromBody] string status)
        {
            return Ok(await _memberService.ChangeStatus(id, status));

        }
        [SwaggerOperation("Change status of member according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(bool))]
        [Produces("application/json")]
        [Route(APIEndPoints.MEMBER_ACTIVATE_V1)]
        [HttpPut]
        public async Task<IActionResult> ActivateMember(ActivateMemberInputDto inputDto)
        {
            return Ok(await _memberService.ActivateMember(inputDto));
        }
    }
}