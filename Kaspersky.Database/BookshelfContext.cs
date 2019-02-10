using Kaspersky.Database.Configuration;
using Kaspersky.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kaspersky.Database
{
    public sealed class BookshelfContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Thumbnail> Thumbnails { get; set; }

        public BookshelfContext( DbContextOptions<BookshelfContext> options )
            : base( options )
        {
        }

        protected override void OnModelCreating( ModelBuilder modelBuilder )
            => modelBuilder
            .ApplyConfiguration<Book, BookConfiguration>()
            .ApplyConfiguration<Author, AuthorConfiguration>()
            .ApplyConfiguration<Thumbnail, ThumbnailConfiguration>();
    }

    internal static class ModelBuilderExtensions
    {
        public static ModelBuilder ApplyConfiguration<TEntityType, TConfigurationType>( this ModelBuilder modelBuilder )
            where TEntityType : class
            where TConfigurationType : IEntityTypeConfiguration<TEntityType>, new()
            => modelBuilder.ApplyConfiguration( new TConfigurationType() );
    }
}
