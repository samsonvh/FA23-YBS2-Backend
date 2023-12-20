using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS2.Data.Models;

namespace YBS2.Data.Configs
{
    public class AccountConfig : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Account");
            builder.HasKey(account => account.Id);
            builder.Property(account => account.Id).ValueGeneratedOnAdd();
            builder.Property(account => account.Username).HasColumnType("nvarchar(50)");
            builder.Property(account => account.Email).HasColumnType("nvarchar(100)");
            builder.Property(account => account.Password).HasColumnType("nvarchar(max)");
            builder.Property(account => account.Role).HasColumnType("nvarchar(20)");
        }
    }
}