using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace YBS.Middlewares
{
    public class RoleAuthorizationAttribute : TypeFilterAttribute
    {
        public RoleAuthorizationAttribute(string role) : base(typeof(RoleAuthorizationFilter))
        {
            Arguments = new object[] { role };
        }
    }
}