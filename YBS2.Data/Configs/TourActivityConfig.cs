using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS2.Data.Models;

namespace YBS2.Data.Configs
{
    public class TourActivityConfig : IEntityTypeConfiguration<TourActivity>
    {
        public void Configure(EntityTypeBuilder<TourActivity> builder)
        {
            builder.ToTable("TourActivity");
            builder.HasKey(tourActivity => tourActivity.Id);
            builder.Property(tourActivity => tourActivity.Id).ValueGeneratedOnAdd();
            builder.Property(tourActivity => tourActivity.Name).HasColumnType("nvarchar(100)");
            builder.Property(tourActivity => tourActivity.Location).HasColumnType("nvarchar(100)").IsRequired(false);
            builder.Property(tourActivity => tourActivity.StartTime).HasColumnType("time");
            builder.Property(tourActivity => tourActivity.EndTime).HasColumnType("time");
            builder.Property(tourActivity => tourActivity.Description).HasColumnType("nvarchar(500)");
            
        }
    }
}