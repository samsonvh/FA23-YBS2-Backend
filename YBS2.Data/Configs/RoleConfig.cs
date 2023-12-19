using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS2.Data.Models;

namespace YBS2.Data.Configs
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");
            builder.HasKey(role => role.Id);
            builder.Property(role => role.Id).ValueGeneratedOnAdd();
            builder.Property(role => role.Name).HasColumnType("nvarchar(10)");
        }
    }
}