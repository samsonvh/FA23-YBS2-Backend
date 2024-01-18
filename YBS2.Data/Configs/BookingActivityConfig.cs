using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS2.Data.Models;

namespace YBS2.Data.Configs
{
    public class BookingActivityConfig : IEntityTypeConfiguration<BookingActivity>
    {
        public void Configure(EntityTypeBuilder<BookingActivity> builder)
        {
            builder.ToTable("BookingActivity");
            builder.HasKey(bookingActivity => bookingActivity.Id);
            builder.Property(bookingActivity => bookingActivity.Id).ValueGeneratedOnAdd();
            builder.Property(bookingActivity => bookingActivity.Name).HasColumnType("nvarchar(100)");
            builder.Property(bookingActivity => bookingActivity.Location).HasColumnType("nvarchar(100)").IsRequired(false);
            builder.Property(bookingActivity => bookingActivity.StartTime).HasColumnType("time");
            builder.Property(bookingActivity => bookingActivity.EndTime).HasColumnType("time");
            builder.Property(bookingActivity => bookingActivity.Description).HasColumnType("nvarchar(500)");
            
        }

    }
}