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
    [Route(APIEndPoints.MEMBER_V1)]
    [RoleAuthorization($"{nameof(EnumRole.Admin)}")]
    public class MemberController : ControllerBase
    {
        private readonly ILogger<MemberController> _logger;
        private readonly IMemberService _memberService;

        public MemberController(ILogger<MemberController> logger, IMemberService memberService)
        {
            _logger = logger;
            _memberService = memberService;
        }
        [SwaggerOperation("Get list of members, paging information")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(DefaultPageResponse<MemberListingDto>))]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] MemberPageRequest pageRequest)
        {
            return Ok(await _memberService.GetAll(pageRequest));
        }

        [SwaggerOperation("Get details of a member according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(MemberDto))]
        [Produces("application/json")]
        [Route(APIEndPoints.MEMBER_ID_V1)]
        [HttpGet]
        [RoleAuthorization($"{nameof(EnumRole.Admin)},{nameof(EnumRole.Member)}")]
        public async Task<IActionResult> GetDetails([FromRoute] Guid id)
        {
            MemberDto? membershipPackageDto = await _memberService.GetDetails(id);
            if (membershipPackageDto != null)
            {
                return Ok(membershipPackageDto);
            }
            else
            {
                return Ok();
            }
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
        [Route(APIEndPoints.MEMBER_ID_V1)]
        [HttpPut]
        [RoleAuthorization($"{nameof(EnumRole.Admin)},{nameof(EnumRole.Member)}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] MemberInputDto inputDto)
        {
            return Ok(await _memberService.Update(id, inputDto));
        }

        [SwaggerOperation("Change status of member according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(bool))]
        [Produces("application/json")]
        [Route(APIEndPoints.MEMBER_ID_V1)]
        [HttpPatch]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromBody] string status)
        {
            return Ok(await _memberService.ChangeStatus(id, status));
        }
    }
}