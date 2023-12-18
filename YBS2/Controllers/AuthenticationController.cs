using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YBS2.Service.Dtos.Input;
using YBS2.Service.Services;
using YBS2.Service.Utils;
using static YBS2.Service.Dtos.PageResponses.DefaultAPIResponse;

namespace YBS2.Controllers
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthService _authService;
        public AuthenticationController(ILogger<AuthenticationController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }
        [Route(APIEndPoint.LOGIN_WITH_GOOGLE)]
        [HttpPost]
        public async Task<Response> LoginWithGoogle ([FromForm]string idToken)
        {
            return await _authService.LoginWithGoogle(idToken);
        }
        [Route(APIEndPoint.LOGIN_WITH_EMAIL_PASSWORD)]
        [HttpPost]
        public async Task<Response> LoginWithEmailAndPassword ([FromForm] AuthenticateInputDto authenticateInputDto)
        {
            return await _authService.LoginWithEmailAndPassword(authenticateInputDto);
        }
        // [Route("genpass")]
        // [HttpGet]
        // public async Task<IActionResult> GeneratePass (string genpass)
        // {
        //     var hashedPassword = PasswordUtil.HashPassword(genpass);
        //     return Ok(hashedPassword);
        // }
    }
}