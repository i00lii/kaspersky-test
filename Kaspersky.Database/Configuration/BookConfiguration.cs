using Kaspersky.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kaspersky.Database.Configuration
{
    internal sealed class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure( EntityTypeBuilder<Book> builder )
        {
            const int isbnThreshold = 17;
            const int textThreshold = 30;

            DbEntiryConfiguration<Book>.Value.Configure( builder );

            builder
                .Property( item => item.Isbn )
                .IsRequired()
                .HasMaxLength( isbnThreshold );

            builder
                .Property( item => item.Title )
                .IsRequired()
                .HasMaxLength( textThreshold );

            builder
                .Property( item => item.Publisher )
                .IsRequired( false )
                .HasMaxLength( textThreshold );

            builder
                .Property( item => item.PagesTotal )
                .IsRequired();

            builder
                .Property( item => item.PublicationYear )
                .IsRequired( false );

            builder
                .HasMany( item => item.Authors )
                .WithOne( item => item.Book )
                .HasForeignKey( item => item.BookId )
                .OnDelete( DeleteBehavior.Cascade );

            builder
                .HasMany( item => item.Thumbnails )
                .WithOne( item => item.Book )
                .HasForeignKey( item => item.BookId )
                .OnDelete( DeleteBehavior.Cascade );
        }
    }
}
