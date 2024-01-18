using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS2.Data.Models;

namespace YBS2.Data.Configs
{
    public class TransactionConfig : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transaction");
            builder.HasKey(transaction => transaction.Id);
            builder.Property(transaction => transaction.Id).ValueGeneratedOnAdd();
            builder.Property(transaction => transaction.Code).HasColumnType("nvarchar(50)").IsRequired(false);
            builder.Property(transaction => transaction.Name).HasColumnType("nvarchar(100)");
            builder.Property(transaction => transaction.BankCode).HasColumnType("nvarchar(50)").IsRequired(false);
            builder.Property(transaction => transaction.BankTranNo).HasColumnType("nvarchar(50)").IsRequired(false);
            builder.Property(transaction => transaction.CardType).HasColumnType("nvarchar(50)").IsRequired(false);
            builder.Property(transaction => transaction.VNPayCode).HasColumnType("nvarchar(50)").IsRequired(false);
        }
    }
}