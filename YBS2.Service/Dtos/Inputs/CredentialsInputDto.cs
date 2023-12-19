using System.ComponentModel.DataAnnotations;
using YBS2.Service.Swaggers;

namespace YBS2.Service.Dtos.Inputs
{
    public class CredentialsInputDto
    {
        [SwaggerSchemaExample("abc@mail.com")]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}