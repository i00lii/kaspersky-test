using AutoMapper;
using Kaspersky.Api.Bookshelf;
using Kaspersky.Api.Bookshelf.Models;
using Kaspersky.Api.Bookshelf.Service;
using Kaspersky.Database;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Kaspersky.Tests.Bookshelf
{
    [TestFixture]
    public sealed partial class BookshelfServiceTest
    {
        private static Book GenerateBook()
            => new Book()
            {
                Title = "Отверженные",
                Isbn = "978-5-389-04911-6",
                PagesTotal = 1248,
                PublicationYear = 2013,
                Publisher = "Азбука",

                Authors = new[]
                {
                    new Author()
                    {
                        Name = "Виктор",
                        Surname = "Гюго"
                    }
                }
            };

        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration
            (
                options =>
                {
                    options.AddProfile<Author.Profile>();
                    options.AddProfile<Book.Profile>();
                    options.AddProfile<Thumbnail.Profile>();
                }
            );

            return config.CreateMapper();
        }

        private static (DbConnection, Func<BookshelfContext>) CreateDbContext()
        {
            DbConnection connection = new SqliteConnection( "DataSource=:memory:" );
            connection.Open();

            var options = new DbContextOptionsBuilder<BookshelfContext>()
                .UseSqlite( connection )
                .Options;

            var context = new BookshelfContext( options );
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            context.Database.EnsureCreated();
            return (connection, () => new BookshelfContext( options ));
        }

        private IMapper _mapper;
        private DbConnection _connection;
        private Func<BookshelfContext> _contextFactory;

        [SetUp]
        public void SetUp()
        {
            _mapper = CreateMapper();
            (_connection, _contextFactory) = CreateDbContext();
        }

        [TearDown]
        public void TearDown() => _connection.Dispose();

        private async Task<T> IteractWithServiceAsync<T>( Func<BookshelfService, Task<T>> lambda )
        {
            using (var context = _contextFactory())
            {
                return await lambda( new BookshelfService( context, _mapper ) );
            }
        }

        private async Task<T> IteractWithDbAsync<T>( Func<BookshelfContext, Task<T>> lambda )
        {
            using (var context = _contextFactory())
            {
                return await lambda( context );
            }
        }



        private Task IteractWithServiceAsync( Func<BookshelfService, Task> lambda )
            => IteractWithServiceAsync( async context => { await lambda( context ); return false; } );

        private Task<Database.Models.Book[]> ScrapeDb()
            => IteractWithDbAsync
            (
                context => context
                    .Books
                    .Include( item => item.Authors )
                    .Include( item => item.Thumbnails )
                    .ToArrayAsync()
            );

        private Task<Guid> GetOnlyBookIdAsync()
            => IteractWithDbAsync( context => context.Books.Select( item => item.Id ).SingleAsync() );
    }
}
