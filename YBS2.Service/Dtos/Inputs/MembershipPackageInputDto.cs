using System.ComponentModel.DataAnnotations;

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
        public string DurationUnit { get; set; }
        [Required]
        public float DiscountPercent { get; set; }
        [Required]
        public string Description { get; set; }
    }
}