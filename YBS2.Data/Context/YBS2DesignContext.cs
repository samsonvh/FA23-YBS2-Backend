using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace YBS2.Data.Context
{
    public class YBS2DesignContext : IDesignTimeDbContextFactory<YBS2Context>
    {
        YBS2Context IDesignTimeDbContextFactory<YBS2Context>.CreateDbContext(string[] args)
        {
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            string dir = Directory.GetParent(Directory.GetCurrentDirectory()).ToString() + "/YBS2";
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(dir)
                .AddJsonFile($"appsettings.{env}.json")
                .Build();
            var connectionStrings = configuration.GetConnectionString("YBS2Context");
            var optionsBuilder = new DbContextOptionsBuilder<YBS2Context>();
            optionsBuilder.UseSqlServer(connectionStrings);

            return new YBS2Context(optionsBuilder.Options);
        }
    }
}