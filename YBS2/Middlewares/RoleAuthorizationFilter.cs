using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Security.Claims;
using YBS2.Service.Exceptions;
using YBS2.Service.Utils;

namespace YBS.Middlewares
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
            var claimsPrincipal = JWTUtils.GetClaim(_httpContextAccessor, _configuration);
            // Retrieve the Role claim value
            if (claimsPrincipal == null)
            {
                throw new APIException(HttpStatusCode.Unauthorized, "Unauthorized", null);
            }
            var roleClaim = claimsPrincipal.FindFirstValue(ClaimTypes.Role);
            if (!_role.Contains(roleClaim))
            {
                throw new APIException(HttpStatusCode.Unauthorized, "Unauthorized", null);
            }
        }
    }
}