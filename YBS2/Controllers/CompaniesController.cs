using Microsoft.AspNetCore.Mvc;
using YBS2.Service.Services;

namespace YBS2.Controllers
{
    [Route(APIEndPoints.COMPANIES_V1)]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly ICompanyService _companyService;

        public CompaniesController(ILogger<AuthenticationController> logger, ICompanyService companyService)
        {
            _logger = logger;
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }

        [Route(APIEndPoints.COMPANIES_ID_V1)]
        [HttpGet]
        public async Task<IActionResult> GetDetails()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update()
        {
            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> ChangeStatus()
        {
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            return Ok();
        }
    }
}
