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
            builder.Property(member => member.AvatarURL).HasColumnType("nvarchar(500)").IsRequired(false);
            builder.Property(member => member.DOB).HasColumnType("date");
            builder.Property(member => member.Address).HasColumnType("nvarchar(200)").IsRequired(false);
            builder.Property(member => member.PhoneNumber).HasColumnType("nvarchar(12)");
            builder.Property(member => member.IdentityNumber).HasColumnType("nvarchar(20)");
            builder.Property(member => member.Nationality).HasColumnType("nvarchar(50)");
            builder.Property(member => member.MemberSinceDate).HasColumnType("datetime");
            builder.Property(member => member.CreatedDate).HasColumnType("datetime");
        }
    }
}