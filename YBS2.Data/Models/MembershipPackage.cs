using System.ComponentModel.DataAnnotations.Schema;
using YBS2.Data.Enums;

namespace YBS2.Data.Models
{
    public class MembershipPackage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Point { get; set; }
        public int Duration { get; set; } 
        public string DurationUnit { get; set; } 
        public float DiscountPercent { get; set; } 
        public string Description { get; set; } 
        public EnumMembershipPackageStatus Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(7);
        public ICollection<MembershipRegistration>? MembershipRegistrations { get; set; } = null;
    }
}