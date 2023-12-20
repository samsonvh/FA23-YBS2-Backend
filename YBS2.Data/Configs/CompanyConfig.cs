using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS2.Data.Models;

namespace YBS2.Data.Configs
{
    public class CompanyConfig : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Company");
            builder.HasKey(company => company.Id);
            builder.Property(company => company.Id).ValueGeneratedOnAdd();
            builder.Property(company => company.Name).HasColumnType("nvarchar(200)");
            builder.Property(company => company.Address).HasColumnType("nvarchar(200)");
            builder.Property(company => company.HotLine).HasColumnType("nvarchar(20)");
            builder.Property(company => company.FacebookURL).HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(company => company.LinkedInURL).HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(company => company.InstagramURL).HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(company => company.LogoURL).HasColumnType("nvarchar(max)").IsRequired(false);
        }
    }
}