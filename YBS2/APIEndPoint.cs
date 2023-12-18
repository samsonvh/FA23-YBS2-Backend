using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS2
{
    public static class APIEndPoint
    {
        //Default Route
        public const string DEFAULT_ROUTE = "api/[controller]";
        //authentication
        public const string LOGIN_WITH_GOOGLE = DEFAULT_ROUTE + "/google";
        public const string LOGIN_WITH_EMAIL_PASSWORD = DEFAULT_ROUTE + "/email-password";
    }
}