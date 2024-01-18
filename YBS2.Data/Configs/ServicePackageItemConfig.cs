using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS2.Data.Models;

namespace YBS2.Data.Configs
{
    public class ServicePackageItemConfig : IEntityTypeConfiguration<ServicePackageItem>
    {
        public void Configure(EntityTypeBuilder<ServicePackageItem> builder)
        {
            builder.ToTable("ServicePackageItem");
            builder.HasKey(servicePackageItem => servicePackageItem.Id);
            builder.Property(servicePackageItem => servicePackageItem.Id).ValueGeneratedOnAdd();
            
        }
    }
}