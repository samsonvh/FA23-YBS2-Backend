using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS2.Data.Models;

namespace YBS2.Data.Configs
{
    public class ServiceConfig : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.ToTable("Service");
            builder.HasKey(service => service.Id);
            builder.Property(service => service.Id).ValueGeneratedOnAdd();
            builder.Property(service => service.Name).HasColumnType("nvarchar(100)");
            builder.Property(service => service.Description).HasColumnType("nvarchar(500)");
            
        }
    }
}