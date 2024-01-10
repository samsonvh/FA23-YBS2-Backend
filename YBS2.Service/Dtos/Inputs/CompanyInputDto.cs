using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YBS2.Service.Dtos.Inputs
{
    public class CompanyInputDto
    {
        [MinLength(2)]
        [Required]
        public string ShortName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Hotline { get; set; }
        public string? FacebookURL { get; set; } = null;
        public string? InstagramURL { get; set; } = null;
        public string? LinkedInURL { get; set; } = null;
        [Required]
        public IFormFile Logo { get; set; }
    }
}
