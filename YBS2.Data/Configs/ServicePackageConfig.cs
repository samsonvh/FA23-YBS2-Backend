using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS2.Data.Models;

namespace YBS2.Data.Configs
{
    public class ServicePackageConfig : IEntityTypeConfiguration<ServicePackage>
    {
        public void Configure(EntityTypeBuilder<ServicePackage> builder)
        {
            builder.ToTable("ServicePackage");
            builder.HasKey(servicePackage => servicePackage.Id);
            builder.Property(servicePackage => servicePackage.Id).ValueGeneratedOnAdd();
            builder.Property(servicePackage => servicePackage.Name).HasColumnType("nvarchar(100)");
            builder.Property(servicePackage => servicePackage.Description).HasColumnType("nvarchar(500)");
            
        }
    }
}