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
    public class MembersController : ControllerBase
    {
        private readonly ILogger<MembersController> _logger;
        private readonly IMemberService _memberService;
        private readonly IConfiguration _configuration;

        public MembersController(ILogger<MembersController> logger, IMemberService memberService, IConfiguration configuration)
        {
            _logger = logger;
            _memberService = memberService;
            _configuration = configuration;
        }

        [SwaggerOperation("[Admin] Get list of members, paging information")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(DefaultPageResponse<MemberListingDto>))]
        [Produces("application/json")]
        [HttpGet]
        [RoleAuthorization(nameof(EnumRole.Admin))]
        public async Task<IActionResult> GetAll([FromQuery] MemberPageRequest pageRequest)
        {
            return Ok(await _memberService.GetAll(pageRequest));
        }

        [SwaggerOperation("[Admin] Get details of a member according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(MemberDto))]
        [Produces("application/json")]
        [Route(APIEndPoints.MEMBER_ID_V1)]
        [HttpGet]
        [RoleAuthorization(nameof(EnumRole.Admin))]
        public async Task<IActionResult> GetDetails([FromRoute] Guid id)
        {
            return Ok(await _memberService.GetDetails(id));
        }

        [SwaggerOperation("[Public] Create new member and return payment URL")]
        [SwaggerResponse(StatusCodes.Status201Created, "Success", typeof(CreateMemberDto))]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] MemberRegistration registration)
        {
            return CreatedAtAction(nameof(Create), await _memberService.Create(registration, HttpContext));
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

        [SwaggerOperation("[Admin] Change status of member according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(bool))]
        [Produces("application/json")]
        [Route(APIEndPoints.MEMBER_ID_V1)]
        [HttpPatch]
        [RoleAuthorization(nameof(EnumRole.Admin))]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromBody] string status)
        {
            return Ok(await _memberService.ChangeStatus(id, status));

        }
        [SwaggerOperation("[Public] Activate member when payment for register successfully")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(bool))]
        [Produces("application/json")]
        [Route(APIEndPoints.MEMBER_ACTIVATE_V1)]
        [HttpGet]
        public async Task<IActionResult> ActivateMember()
        {
            return Ok(await _memberService.ActivateMember(Request.Query));
        }
        [SwaggerOperation("[Public] Create Register Payment URL when login with inactive member account")]
        [SwaggerResponse(StatusCodes.Status201Created, "Success", typeof(string))]
        [Produces("application/json")]
        [Route(APIEndPoints.MEMBER_CREATE_REGISTER_PAYMENT_URL)]
        [HttpPost]
        public async Task<IActionResult> CreateRegisterPaymentURL([FromForm] CreateRegisterPaymentURLInputDto inputDto)
        {
            return CreatedAtAction(nameof(CreateRegisterPaymentURL) ,await _memberService.CreateRegisterPaymentURL(inputDto, HttpContext));
        }
        [SwaggerOperation("Extend Membership")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(MemberDto))]
        [Produces("application/json")]
        [HttpPut]
        [Route(APIEndPoints.MEMBER_EXTEND)]
        [RoleAuthorization(nameof(EnumRole.Member))]
        public async Task<IActionResult> ExtendMembership([FromForm] Guid membershipPackageId)
        {
            ClaimsPrincipal claims = JWTUtils.GetClaim(_configuration, Request.Headers["Authorization"]);
            return Ok(await _memberService.ExtendMembership(claims, membershipPackageId, HttpContext));
        }
        [SwaggerOperation("[Public] Activate extend membership for member when payment for extend membership package successfully")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(bool))]
        [Produces("application/json")]
        [Route(APIEndPoints.MEMBER_ACTIVATE_EXTEND)]
        [HttpGet]
        public async Task<IActionResult> ActivateExtendMember()
        {
            return Ok(await _memberService.ActivateExtendMember(Request.Query));
        }
    }
}