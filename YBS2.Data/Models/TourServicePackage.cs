using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YBS2.Data.Models
{
    public class TourServicePackage
    {
        public Guid Id { get; set; }
        public Guid TourId { get; set; }
        [ForeignKey("TourId")]
        public Tour Tour { get; set; }
        public Guid ServicePackageId { get; set; }
        [ForeignKey("ServicePackageId")]
        public ServicePackage ServicePackage { get; set; }
    }
}