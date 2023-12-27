using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using YBS2.Data.Enums;

namespace YBS2.Data.Models
{
    public class Yacht
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string LOA { get; set; }
        public string BEAM { get; set; }
        public string DRAFT { get; set; }
        public int TotalCrew { get; set; }
        public int TotalPassenger { get; set; }
        public int Cabin { get; set; }
        public int Type { get; set; }
        public EnumYachtStatus Status { get; set; }
        public ICollection<Tour>? Tours { get; set; } = null;
    }
}