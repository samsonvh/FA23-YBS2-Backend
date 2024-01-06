using System.ComponentModel.DataAnnotations;
using YBS2.Data.Enums;

namespace YBS2.Service.Dtos.Inputs
{
    public class MembershipPackageInputDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public int Point { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public EnumTimeUnit DurationUnit { get; set; }
        [Required]
        public float DiscountPercent { get; set; }
        [Required]
        public string Description { get; set; }
    }
}