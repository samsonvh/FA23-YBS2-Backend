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
    [Route(APIEndPoints.TRANSACTION_V1)]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly ITransactionService _transactionService;
        private readonly IConfiguration _configuration;

        public TransactionController(ILogger<TransactionController> logger, ITransactionService transactionService, IConfiguration configuration)
        {
            _logger = logger;
            _transactionService = transactionService;
            _configuration = configuration;
        }

        [SwaggerOperation("[Member] Get list of transactions, paging information")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(DefaultPageResponse<TransactionListingDto>))]
        [Produces("application/json")]
        [HttpGet]
        [RoleAuthorization($"{nameof(EnumRole.Company)},{nameof(EnumRole.Member)}")]
        public async Task<IActionResult> GetAll([FromQuery] TransactionPageRequest pageRequest)
        {
            return Ok(await _transactionService.GetAll(pageRequest));
        }

        [SwaggerOperation("[Member] Get details of a transaction according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(TransactionDto))]
        [Produces("application/json")]
        [Route(APIEndPoints.TRANSACTION_ID_V1)]
        [HttpGet]
        public async Task<IActionResult> GetDetails([FromRoute] Guid id)
        {
            return Ok(await _transactionService.GetDetails(id));
        }

        [SwaggerOperation("[Public] Create new transaction")]
        [SwaggerResponse(StatusCodes.Status201Created, "Success", typeof(TransactionDto))]
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] TransactionInputDto inputDto)
        {
            return CreatedAtAction(nameof(Create) ,await _transactionService.Create(inputDto));
        }

        [SwaggerOperation("[Public] Update transaction details according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(TransactionDto))]
        [Produces("application/json")]
        [HttpPut]
        [Route(APIEndPoints.TRANSACTION_ID_V1)]
        [RoleAuthorization($"{nameof(EnumRole.Company)},{nameof(EnumRole.Member)}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromForm] TransactionInputDto inputDto)
        {
            return Ok(await _transactionService.Update(id, inputDto));
        }

        [SwaggerOperation("[Public] Change status of transaction according to ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(bool))]
        [Produces("application/json")]
        [Route(APIEndPoints.TRANSACTION_ID_V1)]
        [HttpPatch]
        [RoleAuthorization(nameof(EnumRole.Admin))]
        public async Task<IActionResult> ChangeStatus([FromRoute] Guid id, [FromBody] string status)
        {
            return Ok(await _transactionService.ChangeStatus(id, status));

        }
    }
}