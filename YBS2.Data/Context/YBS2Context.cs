using Microsoft.EntityFrameworkCore;
using YBS2.Data.Models;

namespace YBS2.Data.Context
{
    public class YBS2Context : DbContext
    {
        public YBS2Context(DbContextOptions<YBS2Context> options) : base(options)
        {
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Company> Companies { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(YBS2Context).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}