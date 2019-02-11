using AutoMapper;
using Kaspersky.Database;
using System.Threading.Tasks;

namespace Kaspersky.Api.Bookshelf.Service
{
    public sealed partial class BookshelfService : IBookshelfService
    {
        private readonly BookshelfContext _context;
        private readonly IMapper _mapper;

        public BookshelfService( BookshelfContext db, IMapper mapper )
            => (_context, _mapper) = (db, mapper);

        private async Task AddItemAsync<T>( T dbItem )
            where T : class
        {
            await _context.Set<T>().AddAsync( dbItem );
            await _context.SaveChangesAsync();
        }
    }
}
