using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Patisserie.Models
{
    public class PatisserieContextFactory : IDesignTimeDbContextFactory<PatisserieContext>
    {
        PatisserieContext IDesignTimeDbContextFactory<PatisserieContext>.CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<PatisserieContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseMySql(connectionString);
            
            return new PatisserieContext(builder.Options);
        }
    }
}