using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace YBS2.Service.Dtos.Inputs
{
    public class UpdateRequestInputDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string HotLine { get; set; }
        public string? FacebookURL { get; set; } = null;
        public string? LinkedInURL { get; set; } = null;
        public string? InstagramURL { get; set; } = null;
        [Required]
        public IFormFile Logo { get; set; }
    }
}
