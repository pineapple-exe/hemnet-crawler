using Microsoft.EntityFrameworkCore;
using System;
using HemnetCrawler.Domain.Entities;

namespace HemnetCrawler.Data
{
    public class HemnetCrawlerDbContext : DbContext, IDisposable
    {
        public DbSet<Listing> Listings { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<FinalBid> FinalBids { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=localhost;Database=TestUggeboiAndPinegal;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ListingsConfiguration());
            modelBuilder.ApplyConfiguration(new ImagesConfiguration());
            modelBuilder.ApplyConfiguration(new FinalBidConfiguration());
        }
    }
}
