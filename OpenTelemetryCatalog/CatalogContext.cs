using Microsoft.EntityFrameworkCore;
using OpenTelemetryCatalog.Models;

namespace OpenTelemetryCatalog
{
    public class CatalogContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public CatalogContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            options.UseNpgsql(_configuration.GetConnectionString("CatalogDb"));
        }

        public DbSet<Product> Products { get; set; }
    }
}
