using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS2.Data.Models;

namespace YBS2.Data.Configs
{
    public class MembershipPackageConfig : IEntityTypeConfiguration<MembershipPackage>
    {
        public void Configure(EntityTypeBuilder<MembershipPackage> builder)
        {
            builder.ToTable("MembershipPackage");
            builder.HasKey(membershipPackage => membershipPackage.Id);
            builder.Property(membershipPackage => membershipPackage.Id).ValueGeneratedOnAdd();
            builder.Property(membershipPackage => membershipPackage.Name).HasColumnType("nvarchar(100)");
            builder.Property(membershipPackage => membershipPackage.DurationUnit).HasColumnType("nvarchar(10)");
            builder.Property(membershipPackage => membershipPackage.Description).HasColumnType("nvarchar(500)");
            builder.Property(membershipPackage => membershipPackage.CreatedDate).HasColumnType("datetime");
        }
    }
}