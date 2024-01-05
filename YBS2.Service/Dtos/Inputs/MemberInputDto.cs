using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using YBS2.Data.Enums;

namespace YBS2.Service.Dtos.Inputs
{
    public class MemberInputDto
    {
        [Required]
        public Guid MembershipPackageId { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FullName { get; set; }
        public IFormFile? Avatar { get; set; }
        [Required]
        public DateTime DOB { get; set; }
        public string? Address { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string IdentityNumber { get; set; }
        [Required]
        public EnumGender Gender { get; set; }
        [Required]
        public string Nationality { get; set; }
    }
}