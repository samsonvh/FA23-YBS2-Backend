using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YBS2.Service.Dtos.Inputs;
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
        public async Task<IActionResult> GetAll() {
            return Ok();
        }

        [Route(APIEndPoints.UPDATE_REQUESTS_OF_COMPANY_ID_V1)]
        [HttpGet]
        public async Task<IActionResult> GetAllOfCompany([FromQuery] Guid id) {
            return Ok();
        }

        [Route(APIEndPoints.UPDATE_REQUESTS_ID_V1)]
        [HttpGet]
        public async Task<IActionResult> GetDetails([FromQuery] Guid id) {
            return Ok();
        }

        [Route(APIEndPoints.UPDATE_REQUESTS_ID_OF_COMPANY_ID_V1)]
        [HttpGet]
        public async Task<IActionResult> GetDetailsOfCompany([FromQuery] Guid id, [FromQuery] Guid updateRequestId) {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] UpdateRequestInputDto inputDto) {
            
            return Ok(await _updateRequestService.Create(inputDto, Guid.Empty));
        }

        [Route(APIEndPoints.UPDATE_REQUESTS_ID_V1)]
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] Guid id) {
            return Ok();
        }

        [Route(APIEndPoints.UPDATE_REQUESTS_ID_V1)]
        [HttpPatch]
        public async Task<IActionResult> ChangeStatus([FromQuery] Guid id, [FromBody] string status) {
            return Ok();
        }

        [Route(APIEndPoints.UPDATE_REQUESTS_ID_V1)]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Guid id) {
            return Ok();
        }
    }
}
