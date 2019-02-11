using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Kaspersky.Tests.Bookshelf
{
    public sealed partial class BookshelfServiceTest
    {
        [Test]
        public async Task BookAndAuthorsAreRemovedDuringRemoveOperation()
        {
            await IteractWithServiceAsync( service => service.AddAsync( GenerateBook() ) );
            var bookId = await GetOnlyBookIdAsync();

            await IteractWithServiceAsync( service => service.DeleteAsync( bookId ) );
            var counters = await IteractWithDbAsync
            (
                context => Task.WhenAll
                (
                    context.Books.CountAsync(),
                    context.Authors.CountAsync(),
                    context.Thumbnails.CountAsync()
                )
            );

            counters.Should().BeEquivalentTo( new[] { 0, 0, 0 } );
        }
    }
}
