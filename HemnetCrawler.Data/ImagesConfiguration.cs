using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HemnetCrawler.Domain.Entities;

namespace HemnetCrawler.Data
{
    class ImagesConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.Property(e => e.Data).IsRequired();
            builder.Property(e => e.ContentType).IsRequired().HasMaxLength(9);
        }
    }
}
