using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS2.Data.Models;

namespace YBS2.Data.Configs
{
    public class YachtConfig : IEntityTypeConfiguration<Yacht>
    {
        public void Configure(EntityTypeBuilder<Yacht> builder)
        {
            builder.ToTable("Yacht");
            builder.HasKey(yacht => yacht.Id);
            builder.Property(yacht => yacht.Id).ValueGeneratedOnAdd();
            builder.Property(yacht => yacht.Name).HasColumnType("nvarchar(100)");
            builder.Property(yacht => yacht.ImageURL).HasColumnType("nvarchar(500)");
            builder.Property(yacht => yacht.Description).HasColumnType("nvarchar(500)");
            builder.Property(yacht => yacht.Manufacturer).HasColumnType("nvarchar(100)");
            builder.Property(yacht => yacht.LOA).HasColumnType("nvarchar(20)");
            builder.Property(yacht => yacht.BEAM).HasColumnType("nvarchar(20)");
            builder.Property(yacht => yacht.DRAFT).HasColumnType("nvarchar(20)");
        }
    }
}