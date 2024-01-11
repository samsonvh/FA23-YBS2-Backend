using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS2.Data.Models;

namespace YBS2.Data.Configs
{
    public class TourConfig : IEntityTypeConfiguration<Tour>
    {
        public void Configure(EntityTypeBuilder<Tour> builder)
        {
            builder.ToTable("Tour");
            builder.HasKey(tour => tour.Id);
            builder.Property(tour => tour.Id).ValueGeneratedOnAdd();
            builder.Property(tour => tour.Name).HasColumnType("nvarchar(100)");
            builder.Property(tour => tour.ImageURL).HasColumnType("nvarchar(max)");
            builder.Property(tour => tour.Location).HasColumnType("nvarchar(100)");
            builder.Property(tour => tour.StartTime).HasColumnType("time");
            builder.Property(tour => tour.EndTime).HasColumnType("time");
            builder.Property(tour => tour.Description).HasColumnType("nvarchar(500)");
        }

    }
}