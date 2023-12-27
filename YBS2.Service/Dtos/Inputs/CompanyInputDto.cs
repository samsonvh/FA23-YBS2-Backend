using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace YBS2.Service.Dtos.Inputs
{
    public class CompanyInputDto
    {
        [MinLength(2)]
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Hotline { get; set; }
        public string? FacebookURL { get; set; } = null;
        public string? InstagramURL { get; set; } = null;
        public string? LinkedInURL { get; set; } = null;
        public IFormFile Logo { get; set; }
    }
}
