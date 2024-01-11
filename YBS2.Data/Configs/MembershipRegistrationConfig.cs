using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS2.Data.Models;

namespace YBS2.Data.Configs
{
    public class MembershipRegistrationConfig : IEntityTypeConfiguration<MembershipRegistration>
    {
        public void Configure(EntityTypeBuilder<MembershipRegistration> builder)
        {
            builder.ToTable("MembershipRegistration");
            builder.HasKey(membershipRegistration => membershipRegistration.Id);
            builder.Property(membershipRegistration => membershipRegistration.Id).ValueGeneratedOnAdd();
            builder.Property(membershipRegistration => membershipRegistration.Name).HasColumnType("nvarchar(100)").IsRequired(false);
            builder.Property(membershipRegistration => membershipRegistration.DiscountPercent).IsRequired(false);
            builder.Property(membershipRegistration => membershipRegistration.MembershipStartDate).HasColumnType("date");
            builder.Property(membershipRegistration => membershipRegistration.MembershipExpireDate).HasColumnType("date");
        }
    }
}