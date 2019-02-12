using Kaspersky.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kaspersky.Api.Bookshelf.Service
{
    public sealed partial class BookshelfService
    {
        public async Task UpdateAsync( Guid bookId, Models.Book book )
        {
            var oldAuthorIds = await _context
                .Authors
                .Where( item => item.BookId == bookId )
                .Select( item => item.Id )
                .ToArrayAsync();

            _context.Authors.RemoveRange( oldAuthorIds.Select( id => new Author() { Id = id } ) );

            var dbBook = _mapper.Map<Book>( book );
            dbBook.Id = bookId;

            _context.Update( dbBook );
            await _context.SaveChangesAsync();
        }
    }
}
