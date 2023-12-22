using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YBS2.Data.Models
{
    public class UpdateRequest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid? ApproverId { get; set; }
        [ForeignKey("ApproverId")]
        public Account? Account { get; set; }
        public Guid CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string HotLine { get; set; }
        public string? FacebookURL { get; set; } = null;
        public string? LinkedInURL { get; set; } = null;
        public string? InstagramURL { get; set; } = null;
        public string? LogoURL { get; set; } = null;
        public string? Comment { get; set; } = null;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(7);
        public DateTime? ApprovedDate { get; set; } 
    }
}