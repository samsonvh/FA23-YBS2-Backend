using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS2.Data.Enums;

namespace YBS2.Data.Models
{
    public class Service
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public EnumServiceStatus Status { get; set; }
        public ICollection<ServicePackageItem> ServicePackageItems { get; set; }
    }
}