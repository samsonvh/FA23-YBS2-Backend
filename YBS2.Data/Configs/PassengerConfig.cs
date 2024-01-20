using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS2.Data.Models;

namespace YBS2.Data.Configs
{
    public class PassengerConfig : IEntityTypeConfiguration<Passenger>
    {
        public void Configure(EntityTypeBuilder<Passenger> builder)
        {
            builder.ToTable("Passenger");
            builder.HasKey(passenger => passenger.Id);
            builder.Property(passenger => passenger.Id).ValueGeneratedOnAdd();
            builder.Property(passenger => passenger.FullName).HasColumnType("nvarchar(100)");
            builder.Property(passenger => passenger.DOB).HasColumnType("date");
            builder.Property(passenger => passenger.IdentityNumber).HasColumnType("nvarchar(20)").IsRequired(false);
            builder.Property(passenger => passenger.Email).HasColumnType("nvarchar(100)").IsRequired(false);
            builder.Property(passenger => passenger.Address).HasColumnType("nvarchar(200)").IsRequired(false);
            builder.Property(passenger => passenger.SpecialRequest).HasColumnType("nvarchar(500)").IsRequired(false);
        }
    }
}