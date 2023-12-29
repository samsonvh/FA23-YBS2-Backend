using Microsoft.AspNetCore.Mvc;

namespace YBS2.Middlewares.AuthenticationFilter
{
    public class RoleAuthorizationAttribute : TypeFilterAttribute
    {
        public RoleAuthorizationAttribute(string role) : base(typeof(RoleAuthorizationFilter))
        {
            Arguments = new object[] { role };
        }
    }
}