using Microsoft.AspNetCore.Mvc.Filters;
using System.Dynamic;
using System.Net;
using System.Security.Claims;
using YBS2.Service.Exceptions;
using YBS2.Service.Utils;

namespace YBS2.Middlewares.AuthenticationFilter
{
    public class RoleAuthorizationFilter : IAuthorizationFilter
    {
        private readonly string _role;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public RoleAuthorizationFilter(string role, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _role = role;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var claimsPrincipal = JWTUtils.GetClaim(_configuration, _httpContextAccessor.HttpContext.Request.Headers["Authorization"]);
            // Retrieve the Role claim value
            if (claimsPrincipal == null)
            {
                dynamic errors = new ExpandoObject();
                errors.Unauthorized = "Unauthorized";
                throw new APIException(HttpStatusCode.Unauthorized, errors.Unauthorized, errors);
            }
            var roleClaim = TextUtils.Capitalize(claimsPrincipal.FindFirstValue(ClaimTypes.Role));
            if (!_role.Contains(roleClaim))
            {
                dynamic errors = new ExpandoObject();
                errors.Unauthorized = "Unauthorized";
                throw new APIException(HttpStatusCode.Unauthorized, errors.Unauthorized, errors);
            }
        }
    }
}