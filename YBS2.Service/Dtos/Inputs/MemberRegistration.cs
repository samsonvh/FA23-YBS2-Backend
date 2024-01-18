using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS2.Data.Enums;

namespace YBS2.Service.Dtos.Inputs
{
    public class MemberRegistration
    {
        [Required]
        public Guid MembershipPackageId { get; set; }
        public string? DeviceToken { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
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
