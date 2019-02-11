using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Kaspersky.Tests.Bookshelf
{
    public sealed partial class BookshelfServiceTest
    {
        [Test]
        public async Task EverythingToBeAddedIsAddedDuringAddOperation()
        {
            var source = GenerateBook();
            await IteractWithServiceAsync( service => service.AddAsync( source ) );

            var dbBooks = await ScrapeDb();

            dbBooks
                .Should()
                .ContainSingle()
                .Which
                .Should()
                .BeEquivalentTo
                (
                    source,
                    options => options
                        .Excluding( item => item.Authors )
                );

            var dbAuthors = await IteractWithDbAsync( context => context.Authors.OrderBy( item => item.Name ).ToArrayAsync() );
            dbAuthors
                .Should()
                .ContainSingle()
                .Which
                .Should()
                .BeEquivalentTo( source.Authors.Single() );
        }
    }
}
