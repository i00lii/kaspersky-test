using Kaspersky.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kaspersky.Database.Configuration
{
    internal sealed class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure( EntityTypeBuilder<Author> builder )
        {
            const int nameThreshold = 20;

            DbEntiryConfiguration<Author>.Value.Configure( builder );

            builder
                .Property( item => item.Name )
                .IsRequired()
                .HasMaxLength( nameThreshold );

            builder
                .Property( item => item.Surname )
                .IsRequired()
                .HasMaxLength( nameThreshold );
        }
    }
}
