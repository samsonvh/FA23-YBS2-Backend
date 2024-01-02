using System.ComponentModel.DataAnnotations.Schema;
using YBS2.Data.Enums;

namespace YBS2.Data.Models
{
    public class MembershipRegistration
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid MemberId { get; set; }
        [ForeignKey("MemberId")]
        public Member Member { get; set; }
        public Guid MembershipPackageId { get; set; }
        [ForeignKey("MembershipPackageId")]
        public MembershipPackage MembershipPackage { get; set; }
        public string Name { get; set; }
        public float DiscountPercent { get; set; }
        public Transaction Transaction { get; set; }
        public DateTime MembershipStartDate { get; set; }
        public DateTime MembershipExpireDate { get; set; }
        public EnumMembershipRegistrationStatus Status { get; set; }
    }
}