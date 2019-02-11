using FluentAssertions;
using Kaspersky.Api.Bookshelf.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaspersky.Tests.Bookshelf
{
    public sealed partial class BookshelfServiceTest
    {
        [Test]
        public async Task BookListIsSortedcorreclyDuringQueryOperation()
        {
            var bookA = GenerateBook();
            bookA.Title = "A";

            var bookB = GenerateBook();
            bookB.Title = "B";
            bookB.PublicationYear--;

            await IteractWithServiceAsync( item => item.AddAsync( bookA ) );
            await IteractWithServiceAsync( item => item.AddAsync( bookB ) );

            AreEquals( await QueryAsync( QueryRequestSortTargetField.Title, QueryRequestSortDirection.Asc ), bookA, bookB );
            AreEquals( await QueryAsync( QueryRequestSortTargetField.Title, QueryRequestSortDirection.Desc ), bookB, bookA );

            AreEquals( await QueryAsync( QueryRequestSortTargetField.Year, QueryRequestSortDirection.Asc ), bookB, bookA );
            AreEquals( await QueryAsync( QueryRequestSortTargetField.Year, QueryRequestSortDirection.Desc ), bookA, bookB );

            async Task<IReadOnlyCollection<Book>> QueryAsync( QueryRequestSortTargetField field, QueryRequestSortDirection direction )
            {
                var items = await IteractWithServiceAsync( item => item.QueryAsync( field, direction ) );
                return items.Select( item => item.Value ).ToArray();
            }

            void AreEquals( IEnumerable<Book> dbBooks, params Book[] target ) => dbBooks.ToArray().Should().BeEquivalentTo( target );
        }
    }
}
