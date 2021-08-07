using Microsoft.EntityFrameworkCore;
using HemnetCrawler.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HemnetCrawler.Data
{
    class ListingRatingsConfiguration : IEntityTypeConfiguration<ListingRating>
    {
        public void Configure(EntityTypeBuilder<ListingRating> builder)
        {
            builder.Property(e => e.ListingId).IsRequired();
            builder.Property(e => e.KitchenRating);
            builder.Property(e => e.BathroomRating);
        }
    }
}
