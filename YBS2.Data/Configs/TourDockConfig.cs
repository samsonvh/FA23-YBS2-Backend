using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS2.Data.Models;

namespace YBS2.Data.Configs
{
    public class TourDockConfig : IEntityTypeConfiguration<TourDock>
    {
        public void Configure(EntityTypeBuilder<TourDock> builder)
        {
            builder.ToTable("TourDock");
            builder.HasKey(tourDock => tourDock.Id);
            builder.Property(tourDock => tourDock.Id).ValueGeneratedOnAdd();
            builder.HasOne(tourDock => tourDock.Tour).WithMany(tour => tour.TourDocks).HasForeignKey(tourDock => tourDock.TourId);
            builder.HasOne(tourDock => tourDock.Dock).WithMany(dock => dock.TourDocks).IsRequired(false).HasForeignKey(tourDock => tourDock.DockId);
        }
    }
}