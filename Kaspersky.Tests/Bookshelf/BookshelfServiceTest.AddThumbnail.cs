using FluentAssertions;
using Kaspersky.Api.Bookshelf.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Kaspersky.Tests.Bookshelf
{
    public sealed partial class BookshelfServiceTest
    {
        [Test]
        public async Task ThumbnailIsAddedToBookDuringAppropriateOperation()
        {
            var book = GenerateBook();
            var thumbnail = new Thumbnail() { Data = new byte[42] };
            await IteractWithServiceAsync( service => service.AddAsync( book ) );

            var bookId = await IteractWithDbAsync( context => context.Books.Select( item => item.Id ).SingleAsync() );

            await IteractWithServiceAsync( service => service.AddThumbnailAsync( bookId, thumbnail ) );

            var dbThumbnails = await IteractWithDbAsync( context => context.Thumbnails.ToArrayAsync() );
            dbThumbnails.Should().BeEquivalentTo( new[] { thumbnail } );
        }
    }
}
