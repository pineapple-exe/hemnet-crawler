using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HemnetCrawler.Domain.Entities;

namespace HemnetCrawler.Data
{
    class FinalBidConfiguration : IEntityTypeConfiguration<FinalBid>
    {
        public void Configure(EntityTypeBuilder<FinalBid> builder)
        {
            builder.Property(e => e.HemnetId).IsRequired();
            builder.Property(e => e.Href).IsRequired();
            builder.Property(e => e.City).IsRequired();
            builder.Property(e => e.PostalCode);
            builder.Property(e => e.ConstructionYear);
            builder.Property(e => e.DemandedPrice);
            builder.Property(e => e.Price).IsRequired();
            builder.Property(e => e.LandLeaseFee);
            builder.Property(e => e.OwnershipType).IsRequired();
            builder.Property(e => e.PriceDevelopment);
            builder.Property(e => e.Rooms);
            builder.Property(e => e.Street).IsRequired();
            builder.Property(e => e.SoldDate).IsRequired();
            builder.Property(e => e.PricePerSquareMeter);
            builder.Property(e => e.LivingArea);
            builder.Property(e => e.BiArea);
            builder.Property(e => e.LivingArea);
            builder.Property(e => e.PropertyArea);
            builder.Property(e => e.Fee);
            builder.Property(e => e.SoldDate).IsRequired().HasColumnType("Date");
            builder.Property(e => e.LastUpdated).IsRequired();
        }
    }
}
