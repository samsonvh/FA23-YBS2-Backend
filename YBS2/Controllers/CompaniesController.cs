using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using YBS2.Service.Dtos;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Dtos.Listings;
using YBS2.Service.Dtos.PageRequests;
using YBS2.Service.Dtos.PageResponses;
using YBS2.Service.Services;

namespace YBS2.Controllers
{
    [Route(APIEndPoints.COMPANIES_V1)]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ILogger<CompaniesController> _logger;
        private readonly ICompanyService _companyService;

        public CompaniesController(ILogger<CompaniesController> logger, ICompanyService companyService)
        {
            _logger = logger;
            _companyService = companyService;
        }

        [SwaggerOperation("[ADMIN] Get list of companies, paging information")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(DefaultPageResponse<CompanyListingDto>))]
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CompanyPageRequest pageRequest)
        {
            return Ok(await _companyService.GetAll(pageRequest));
        }

        [SwaggerOperation("[ADMIN] Get details of a company according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(CompanyDto))]
        [Produces("application/json")]
        [Route(APIEndPoints.COMPANIES_ID_V1)]
        [HttpGet]
        public async Task<IActionResult> GetDetails([FromRoute] Guid id)
        {
            return Ok(await _companyService.GetDetails(id));
        }

        [SwaggerOperation("[ADMIN] Create new company")]
        [SwaggerResponse(StatusCodes.Status201Created, "Success", typeof(CompanyDto))]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CompanyInputDto inputDto)
        {
            return Ok(await _companyService.Create(inputDto));
        }

        [SwaggerOperation("Update company details according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(CompanyDto))]
        [Produces("application/json")]
        [Route(APIEndPoints.COMPANIES_ID_V1)]
        [NonAction]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] CompanyInputDto inputDto)
        {
            return Ok();
        }

        [SwaggerOperation("[ADMIN] Change status of company according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(bool))]
        [Produces("application/json")]
        [Route(APIEndPoints.COMPANIES_ID_V1)]
        [HttpPatch]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromBody] string status)
        {
            return Ok(await _companyService.ChangeStatus(id, status));
        }

        [SwaggerOperation("[ADMIN] Delete company according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(bool))]
        [Produces("application/json")]
        [Route(APIEndPoints.COMPANIES_ID_V1)]
        [NonAction]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            return Ok();
        }
    }
}
