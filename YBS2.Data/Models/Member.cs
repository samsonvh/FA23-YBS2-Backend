using System.ComponentModel.DataAnnotations.Schema;
using YBS2.Data.Enums;

namespace YBS2.Data.Models
{
    public class Member
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        public string FullName { get; set; }
        public string AvatarURL { get; set; }
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentityNumber { get; set; }
        public EnumGender Gender { get; set; }
        public string Nationality { get; set; }
        public DateTime MembershipStartDate { get; set; }
        public DateTime MembershipExpireDate { get; set; }
        public EnumMemberStatus Status { get; set; }
    }
}