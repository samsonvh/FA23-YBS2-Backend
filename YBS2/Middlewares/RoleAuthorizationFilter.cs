using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using YBS2.Service.Exceptions;
using YBS2.Service.Services;
using YBS2.Service.Utils;

namespace YBS.Middlewares
{
    public class RoleAuthorizationFilter : IAuthorizationFilter
    {
        private readonly string _role;
        public IConfiguration _configuration { get; set; }
        public IHttpContextAccessor _httpContextAccessor { get; set; }
        public RoleAuthorizationFilter(string role, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _role = role;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var claimsPrincipal = JWTUtils.GetClaim(_httpContextAccessor,_configuration);
            // Retrieve the Role claim value
            var roleClaim = claimsPrincipal.FindFirstValue(ClaimTypes.Role);
            if (!_role.Contains(roleClaim))
            {
                throw new APIException(HttpStatusCode.Unauthorized, "Unauthorized");
            }
        }
    }
}