using Microsoft.AspNetCore.Http;

namespace YBS2.Service.Dtos.Inputs
{
    public class CompanyInputDto
    {
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
