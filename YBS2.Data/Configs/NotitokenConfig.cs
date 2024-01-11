using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS2.Data.Models;

namespace YBS2.Data.Configs
{
    public class NotitokenConfig : IEntityTypeConfiguration<Notitoken>
    {
        public void Configure(EntityTypeBuilder<Notitoken> builder)
        {
            builder.ToTable("Notitoken");
            builder.HasKey(notitoken => notitoken.Id);
            builder.Property(notitoken => notitoken.Id).ValueGeneratedOnAdd();
            builder.Property(notitoken => notitoken.LoggedinDevice).HasColumnType("nvarchar(50)");
        }
    }
}