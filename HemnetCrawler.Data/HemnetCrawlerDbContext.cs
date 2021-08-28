using Microsoft.EntityFrameworkCore;
using HemnetCrawler.Domain.Entities;

namespace HemnetCrawler.Data
{
    public class HemnetCrawlerDbContext : DbContext
    {
        public DbSet<Listing> Listings { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ListingRating> ListingRatings { get; set; }
        public DbSet<FinalBid> FinalBids { get; set; }

        public HemnetCrawlerDbContext()
        {

        }

        public HemnetCrawlerDbContext(DbContextOptions<HemnetCrawlerDbContext> contextOptions) : base(contextOptions)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=localhost;Database=CleanTestUggeboiAndPinegal;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ListingsConfiguration());
            modelBuilder.ApplyConfiguration(new ImagesConfiguration());
            modelBuilder.ApplyConfiguration(new ListingRatingsConfiguration());
            modelBuilder.ApplyConfiguration(new FinalBidConfiguration());
        }
    }
}
