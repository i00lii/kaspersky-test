using Kaspersky.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kaspersky.Database.Configuration
{
    internal sealed class ThumbnailConfiguration : IEntityTypeConfiguration<Thumbnail>
    {
        public void Configure( EntityTypeBuilder<Thumbnail> builder )
        {
            DbEntiryConfiguration<Thumbnail>.Value.Configure( builder );

            builder
                .Property( item => item.Data )
                .IsRequired();
        }
    }
}
