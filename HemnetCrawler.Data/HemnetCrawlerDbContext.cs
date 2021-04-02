using Microsoft.EntityFrameworkCore;
using HemnetCrawler.Data.Entities;

namespace HemnetCrawler.Data
{
    class HemnetCrawlerDbContext : DbContext
    {
        public DbSet<Listing> Listings { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=localhost;Database=UggeboiAndPinegal;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ListingsConfiguration());
            modelBuilder.ApplyConfiguration(new ImagesConfiguration());
        }
    }
}
