using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using YBS2.Data.Enums;

namespace YBS2.Data.Models
{
    public class ServicePackage
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public EnumServicePackageStatus Status { get; set; }
        public ICollection<ServicePackageItem> ServicePackageItems { get; set; }
        public ICollection<TourServicePackage>? TourServicePackages { get; set; }
    }
}