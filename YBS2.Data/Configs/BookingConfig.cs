using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS2.Data.Models;

namespace YBS2.Data.Configs
{
    public class BookingConfig : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Booking");
            builder.HasKey(booking => booking.Id);
            builder.Property(booking => booking.Id).ValueGeneratedOnAdd();
            builder.Property(booking => booking.BookingDate).HasColumnType("datetime");
            builder.Property(booking => booking.TotalPassengers).HasColumnType("int");
            builder.Property(booking => booking.Note).HasColumnType("nvarchar(500)").IsRequired(false);
            builder.Property(booking => booking.CreatedDate).HasColumnType("datetime");
            builder.Property(booking => booking.DeviceToken).HasColumnType("nvarchar(255)").IsRequired(false);
            builder.Property(booking => booking.TourName).HasColumnType("nvarchar(100)");
            builder.Property(booking => booking.Location).HasColumnType("nvarchar(100)");
            builder.Property(booking => booking.StartTime).HasColumnType("time");
            builder.Property(booking => booking.EndTime).HasColumnType("time");
            builder.Property(passenger => passenger.SpecialRequest).HasColumnType("nvarchar(500)").IsRequired(false);  
        }
    }
}