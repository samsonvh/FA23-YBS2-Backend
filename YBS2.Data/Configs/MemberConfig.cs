using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS2.Data.Models;

namespace YBS2.Data.Configs
{
    public class MemberConfig : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.ToTable("Member");
            builder.HasKey(member => member.Id);
            builder.Property(member => member.Id).ValueGeneratedOnAdd();
            builder.Property(member => member.FullName).HasColumnType("nvarchar(100)");
            builder.Property(member => member.AvatarURL).HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(member => member.DOB).HasColumnType("date");
            builder.Property(member => member.Address).HasColumnType("nvarchar(200)");
            builder.Property(member => member.PhoneNumber).HasColumnType("nvarchar(11)");
            builder.Property(member => member.Nationality).HasColumnType("nvarchar(20)");
            builder.Property(member => member.MembershipStartDate).HasColumnType("date");
            builder.Property(member => member.MembershipExpireDate).HasColumnType("date");
        }
    }
}