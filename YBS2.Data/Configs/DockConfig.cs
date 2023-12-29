using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS2.Data.Models;

namespace YBS2.Data.Configs
{
    public class DockConfig : IEntityTypeConfiguration<Dock>
    {
        public void Configure(EntityTypeBuilder<Dock> builder)
        {
            builder.ToTable("Dock");
            builder.HasKey(dock => dock.Id);
            builder.Property(dock => dock.Id).ValueGeneratedOnAdd();
            builder.Property(dock => dock.Name).HasColumnType("nvarchar(100)");
            builder.Property(dock => dock.Address).HasColumnType("nvarchar(200)");
            builder.Property(dock => dock.ImageURL).HasColumnType("nvarchar(500)");
            builder.Property(dock => dock.Description).HasColumnType("nvarchar(500)");
        }
    }
}