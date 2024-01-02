using System.ComponentModel.DataAnnotations.Schema;
using YBS2.Data.Enums;

namespace YBS2.Data.Models
{
    public class Member
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        public Wallet Wallet { get; set; }
        public string FullName { get; set; }
        public string? AvatarURL { get; set; } = null;
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentityNumber { get; set; }
        public EnumGender Gender { get; set; }
        public string Nationality { get; set; }
        public DateTime? MemberSinceDate { get; set; }
        public EnumMemberStatus Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(7);
        public ICollection<MembershipRegistration> MembershipRegistrations { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
    }
}