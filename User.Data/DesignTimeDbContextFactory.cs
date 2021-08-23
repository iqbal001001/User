using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace User.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<UserContext>
    {
        public UserContext CreateDbContext(string[] args)
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfigurationRoot configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
               //.AddJsonFile(@Directory.GetCurrentDirectory() + "/../MyCookingMaster.API/appsettings.json")
              .AddJsonFile("appsettings.json", optional: false)
              .AddJsonFile($"appsettings.{envName}.json", optional: false)
              .Build();

            var builder = new DbContextOptionsBuilder<UserContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new UserContext(builder.Options);
        }
    }
}
