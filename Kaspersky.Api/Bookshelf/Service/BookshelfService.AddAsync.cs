using Kaspersky.Database.Models;
using System.Threading.Tasks;

namespace Kaspersky.Api.Bookshelf.Service
{
    public sealed partial class BookshelfService
    {
        public Task AddAsync( Models.Book book ) => AddItemAsync( _mapper.Map<Book>( book ) );
    }
}
