using Kaspersky.Database.Models;
using System;
using System.Threading.Tasks;

namespace Kaspersky.Api.Bookshelf.Service
{
    public sealed partial class BookshelfService
    {
        public Task AddThumbnailAsync( Guid bookId, Models.Thumbnail thumbnail )
        {
            var dbItem = _mapper.Map<Thumbnail>( thumbnail );
            dbItem.BookId = bookId;

            return AddItemAsync( dbItem );
        }
    }
}
