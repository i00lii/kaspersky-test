using FluentAssertions;
using Kaspersky.Api.Bookshelf.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kaspersky.Tests.Bookshelf
{
    public sealed partial class BookshelfServiceTest
    {
        [Test]
        public async Task AuthorListIsUpdatedDuringUpdateOperaiton()
        {
            var book = GenerateBook();
            await IteractWithServiceAsync( service => service.AddAsync( book ) );
            var bookId = await GetOnlyBookIdAsync();


            book.Authors = new Author[]
            {
                new Author()
                {
                    Name = "Bob",
                    Surname = "Dilan"
                },
                new Author()
                {
                    Name = "Stan",
                    Surname = "Smith"
                },

            };

            await IteractWithServiceAsync( service => service.UpdateAsync( bookId, book ) );

            var dbAuthors = await IteractWithDbAsync( context => context.Authors.OrderBy( item => item.Name ).ToArrayAsync() );

            dbAuthors
                .Should()
                .HaveCount( book.Authors.Count );

            foreach (var (dbAuthor, sourceAuthor) in Enumerable.Zip( book.Authors, dbAuthors, ( a, b ) => (a, b) ))
            {
                dbAuthor
                    .Should()
                    .BeEquivalentTo
                    (
                        sourceAuthor,
                        options => options
                            .Excluding( item => item.Id )
                            .Excluding( item => item.BookId )
                            .Excluding( item => item.Book )
                    );
            };
        }
    }
}
