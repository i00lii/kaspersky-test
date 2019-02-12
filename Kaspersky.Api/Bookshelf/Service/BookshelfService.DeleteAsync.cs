using Kaspersky.Database.Models;
using System;
using System.Threading.Tasks;

namespace Kaspersky.Api.Bookshelf.Service
{
    public sealed partial class BookshelfService
    {
        public Task DeleteAsync( Guid bookId )
        {
            _context.Books.Remove( new Book() { Id = bookId } );
            return _context.SaveChangesAsync();
        }
    }
}
