using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS2.Data.Models;

namespace YBS2.Data.Configs
{
    public class UpdateRequestConfig : IEntityTypeConfiguration<UpdateRequest>
    {
        public void Configure(EntityTypeBuilder<UpdateRequest> builder)
        {
            builder.ToTable("UpdateRequest");
            builder.HasKey(updateRequest => updateRequest.Id);
            builder.Property(updateRequest => updateRequest.Id).ValueGeneratedOnAdd();
            builder.Property(updateRequest => updateRequest.Name).HasColumnType("nvarchar(200)");
            builder.Property(updateRequest => updateRequest.Address).HasColumnType("nvarchar(200)");
            builder.Property(updateRequest => updateRequest.HotLine).HasColumnType("nvarchar(12)");
            builder.Property(updateRequest => updateRequest.FacebookURL).HasColumnType("nvarchar(500)").IsRequired(false);
            builder.Property(updateRequest => updateRequest.LinkedInURL).HasColumnType("nvarchar(500)").IsRequired(false);
            builder.Property(updateRequest => updateRequest.InstagramURL).HasColumnType("nvarchar(500)").IsRequired(false);
            builder.Property(updateRequest => updateRequest.LogoURL).HasColumnType("nvarchar(500)").IsRequired(false);
            builder.Property(updateRequest => updateRequest.Comment).HasColumnType("nvarchar(500)").IsRequired(false);
            builder.Property(updateRequest => updateRequest.CreatedDate).HasColumnType("datetime");
            builder.Property(updateRequest => updateRequest.ApprovedDate).HasColumnType("datetime").IsRequired(false);
        }
    }
}