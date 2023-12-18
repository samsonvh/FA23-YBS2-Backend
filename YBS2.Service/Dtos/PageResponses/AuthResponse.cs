using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS2.Service.Dtos.PageResponses
{
    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public int AccountId { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}