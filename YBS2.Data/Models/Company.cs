using System.ComponentModel.DataAnnotations.Schema;

namespace YBS2.Data.Models
{
    public class Company
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string HotLine { get; set; }
        public string? FacebookURL { get; set; } = null;
        public string? LinkedInURL { get; set; } = null;
        public string? InstagramURL { get; set; } = null;
        public string? LogoURL { get; set; } = null;
        public DateTime LastUpdatedDate { get; set; } = DateTime.UtcNow.AddHours(7);
        public ICollection<UpdateRequest>? UpdateRequests { get; set; } = null;
        public ICollection<Dock> Docks { get; set; } 
        public ICollection<Tour> Tours { get; set; } 
        public ICollection<Yacht> Yachts { get; set; }
    }
}