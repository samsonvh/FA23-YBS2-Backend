using Microsoft.AspNetCore.Mvc;
using YBS.Middlewares;
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
        [RoleAuthorization("Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }
        [RoleAuthorization("Admin,Company")]
        [Route(APIEndPoints.COMPANIES_ID_V1)]
        [HttpGet]
        public async Task<IActionResult> GetDetails()
        {
            return Ok();
        }

        [HttpPost]
        [RoleAuthorization("Admin")]
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
        [RoleAuthorization("Admin")]
        public async Task<IActionResult> ChangeStatus()
        {
            return Ok();
        }

        [HttpDelete]
        [RoleAuthorization("Admin")]
        public async Task<IActionResult> Delete()
        {
            return Ok();
        }
    }
}
