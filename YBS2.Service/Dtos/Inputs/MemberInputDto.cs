using Microsoft.AspNetCore.Http;
using YBS2.Data.Enums;

namespace YBS2.Service.Dtos.Inputs
{
    public class MemberInputDto
    {
        public string FullName { get; set; }
        public IFormFile? AvatarURL { get; set; } = null;
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentityNumber { get; set; }
        public EnumGender Gender { get; set; }
        public string Nationality { get; set; }
        public int MembershipPackageId { get; set; }
    }
}