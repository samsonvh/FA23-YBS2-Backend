using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using YBS2.Service.Services;

namespace YBS2.Controllers
{
    [ApiController]
    
    public class VNPayController : ControllerBase
    {
        private readonly ILogger<VNPayController> _logger;
        private readonly IVNPayService _vnpayService;

        public VNPayController(ILogger<VNPayController> logger, IVNPayService vnpayService)
        {
            _logger = logger;
            _vnpayService = vnpayService;
        }
        [SwaggerOperation("Create payment url of register membership")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(string))]
        [Produces("application/json")]
        [HttpPost]
        [Route(APIEndPoints.VNPAY_REGISTER_V1)]
        public async Task<IActionResult> CreateRegisterRequestURL ([FromForm] Guid membershipPackageId)
        {
            return Ok(await _vnpayService.CreateRegisterRequestURL(membershipPackageId, HttpContext));
        }
        [SwaggerOperation("Create payment url of register membership")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(string))]
        [Produces("application/json")]
        [HttpGet]
        [Route(APIEndPoints.VNPAY_REGISTER_V1)]
        public async Task<IActionResult> CallBackRegisterPayment ()
        {
            return Ok(await _vnpayService.CallBackRegisterPayment(Request.Query));
        }
    }
}