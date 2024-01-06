using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using YBS2.Service.Dtos.Details;
using YBS2.Service.Dtos.Inputs;
using YBS2.Service.Services;

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

        [SwaggerOperation(Summary = "Authenticate using IdToken from Google Service")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successfully Authenticated", typeof(AuthResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid IdToken", typeof(string))]
        [Produces("application/json")]
        [Route(APIEndPoints.AUTHENTICATION_GOOGLE_V1)]
        [HttpPost]
        public async Task<IActionResult> LoginWithGoogle([FromBody] string idToken)
        {
            return Ok(await _authService.LoginWithGoogle(idToken));
        }

        [SwaggerOperation(Summary = "Authenticate using Email & Password")]
        [SwaggerResponse(StatusCodes.Status200OK, "Successfully Authenticated", typeof(AuthResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid Email", typeof(string))]
        [Produces("application/json")]
        [Route(APIEndPoints.AUTHENTICATION_CREDENTIALS_V1)]
        [HttpPost]
        public async Task<IActionResult> LoginWithCredentials([FromForm] CredentialsInputDto credentials)
        {
            return Ok(await _authService.LoginWithCredentials(credentials));
        }
    }
}