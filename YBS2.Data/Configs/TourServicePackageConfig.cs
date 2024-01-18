using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS2.Data.Models;

namespace YBS2.Data.Configs
{
    public class TourServicePackageConfig : IEntityTypeConfiguration<TourServicePackage>
    {
        public void Configure(EntityTypeBuilder<TourServicePackage> builder)
        {
            builder.ToTable("TourServicePackage");
            builder.HasKey(tourServicePackage => tourServicePackage.Id);
            builder.Property(tourServicePackage => tourServicePackage.Id).ValueGeneratedOnAdd();
            builder.HasOne(tourServicePackage => tourServicePackage.Tour).WithMany(tour => tour.TourServicePackages).IsRequired(false).HasForeignKey(tourServicePackage => tourServicePackage.TourId);
            builder.HasOne(tourServicePackage => tourServicePackage.ServicePackage).WithMany(servicePackage => servicePackage.TourServicePackages).IsRequired(false).HasForeignKey(tourServicePackage => tourServicePackage.ServicePackageId);
        }

    }
}