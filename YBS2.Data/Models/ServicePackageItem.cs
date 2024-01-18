using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YBS2.Data.Models
{
    public class ServicePackageItem
    {
        public Guid Id { get; set; }
        public Guid ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public Service Service { get; set; }
        public Guid ServicePackageId { get; set; }
        [ForeignKey("ServicePackageId")]
        public ServicePackage ServicePackage { get; set; }
    }
}