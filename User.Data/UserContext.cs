using User.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace User.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public UserContext() : base()
        {
        }

        public DbSet<UserInfo> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserInfo>()
           .Property(b => b.FirstName).HasMaxLength(200);
           builder.Entity<UserInfo>()
           .Property(b => b.LastName).HasMaxLength(200);
            builder.Entity<UserInfo>()
           .Property(b => b.Email).HasMaxLength(200);

            builder.Entity<UserInfo>()
          .Property(b => b.UpdatedOn).HasMaxLength(200);
            builder.Entity<UserInfo>()
          .Property(b => b.CreatedOn).HasMaxLength(200);

            builder.Entity<UserInfo>().HasData(SeedUserData());
        }

        public List<UserInfo> SeedUserData()
        {
            var users = new List<UserInfo>();
            using (StreamReader r = new StreamReader(@"Mock_Data.json"))
            {
                string json = r.ReadToEnd();
                var settings = new JsonSerializerSettings();
                settings.DateFormatString = "YYYY-MM-DD";
                settings.ContractResolver = new CustomContractResolver();
                
                users = JsonConvert.DeserializeObject<List<UserInfo>>(json, settings);
                users.ForEach(u => u.UpdatedOn = u.CreatedOn = DateTime.Now);
                users.ForEach(u => u.UpdatedBy = u.CreatedBy = "System");
            }
            return users;
        }
    }

    public class CustomContractResolver : DefaultContractResolver
    {
        private Dictionary<string, string> PropertyMappings { get; set; }

        public CustomContractResolver()
        {
            this.PropertyMappings = new Dictionary<string, string>
        {
            {"FirstName", "first_name"},
            {"LastName", "Last_name"},
            {"Email", "email"},
            {"Gender", "gender"},
            {"Status", "status"}
        };
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            string resolvedName = null;
            var resolved = this.PropertyMappings.TryGetValue(propertyName, out resolvedName);
            return (resolved) ? resolvedName : base.ResolvePropertyName(propertyName);
        }
    }
}