using Microsoft.AspNetCore.Mvc;
using YBS2.Data.Enums;
using YBS2.Middlewares.AuthenticationFilter;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.PageRequests;
using YBS2.Service.Services;

namespace YBS2.Controllers
{
    [Route(APIEndPoints.UPDATE_REQUESTS_V1)]
    [ApiController]
    public class UpdateRequestsController : ControllerBase
    {
        private readonly ILogger<UpdateRequestsController> _logger;
        private readonly IUpdateRequestService _updateRequestService;

        public UpdateRequestsController(ILogger<UpdateRequestsController> logger, IUpdateRequestService updateRequestService)
        {
            _logger = logger;
            _updateRequestService = updateRequestService;
        }

        [HttpGet]
        [RoleAuthorization(nameof(EnumRole.Admin))]
        public async Task<IActionResult> GetAll([FromQuery] UpdateRequestPageRequest pageRequest)
        {
            return Ok(await _updateRequestService.GetAll(pageRequest));
        }

        [Route(APIEndPoints.UPDATE_REQUESTS_OF_COMPANY_ID_V1)]
        [HttpGet]
        [RoleAuthorization($"{nameof(EnumRole.Admin)}, {nameof(EnumRole.Company)}")]
        public async Task<IActionResult> GetAllOfCompany([FromRoute] Guid id, [FromQuery] UpdateRequestPageRequest pageRequest)
        {
            return Ok(await _updateRequestService.GetAll(pageRequest, id));
        }

        [Route(APIEndPoints.UPDATE_REQUESTS_ID_V1)]
        [HttpGet]
        public async Task<IActionResult> GetDetails([FromRoute] Guid id)
        {
            return Ok(await _updateRequestService.GetDetails(id));
        }

        [Route(APIEndPoints.UPDATE_REQUESTS_ID_OF_COMPANY_ID_V1)]
        [HttpGet]
        public async Task<IActionResult> GetDetailsOfCompany([FromRoute] Guid id, [FromRoute] Guid updateRequestId)
        {
            return Ok(await _updateRequestService.GetDetails(updateRequestId, id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] UpdateRequestInputDto inputDto)
        {

            return Ok(await _updateRequestService.Create(inputDto, Guid.Empty));
        }

        [Route(APIEndPoints.UPDATE_REQUESTS_ID_V1)]
        [HttpPut]
        public async Task<IActionResult> Update([FromRoute] Guid id)
        {
            return Ok();
        }

        [Route(APIEndPoints.UPDATE_REQUESTS_ID_V1)]
        [HttpPatch]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromBody] string status)
        {
            return Ok();
        }

        [Route(APIEndPoints.UPDATE_REQUESTS_ID_V1)]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            return Ok();
        }
    }
}
