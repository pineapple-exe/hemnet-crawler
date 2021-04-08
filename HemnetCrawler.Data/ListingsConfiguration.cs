using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HemnetCrawler.Data.Entities;

namespace HemnetCrawler.Data
{
    class ListingsConfiguration : IEntityTypeConfiguration<Listing>
    {
        public void Configure(EntityTypeBuilder<Listing> builder)
        {
            builder.Property(e => e.HemnetId).IsRequired();
            builder.Property(e => e.NewConstruction).IsRequired();
            builder.Property(e => e.Street).IsRequired().HasMaxLength(96);
            builder.Property(e => e.City).IsRequired().HasMaxLength(96);
            builder.Property(e => e.Price);
            builder.Property(e => e.PricePerSquareMeter);
            builder.Property(e => e.Description).IsRequired();
            builder.Property(e => e.HomeType).IsRequired().HasMaxLength(22);
            builder.Property(e => e.OwnershipType).IsRequired().HasMaxLength(11);
            builder.Property(e => e.Rooms);
            builder.Property(e => e.Balcony);
            builder.Property(e => e.Floor);
            builder.Property(e => e.LivingArea);
            builder.Property(e => e.BiArea);
            builder.Property(e => e.PropertyArea);
            builder.Property(e => e.Fee);
            builder.Property(e => e.ConstructionYear);
            builder.Property(e => e.HomeOwnersAssociation);
            builder.Property(e => e.Utilities);
            builder.Property(e => e.EnergyClassification).HasMaxLength(16);
            builder.Property(e => e.Visits).IsRequired();
            builder.Property(e => e.DaysOnHemnet).IsRequired();
        }
    }
}
