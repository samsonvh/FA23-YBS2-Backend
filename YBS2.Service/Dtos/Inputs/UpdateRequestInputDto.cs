using Microsoft.AspNetCore.Http;

namespace YBS2.Service.Dtos.Inputs
{
    public class UpdateRequestInputDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string HotLine { get; set; }
        public string? FacebookURL { get; set; } = null;
        public string? LinkedInURL { get; set; } = null;
        public string? InstagramURL { get; set; } = null;
        public IFormFile Logo { get; set; }
    }
}
