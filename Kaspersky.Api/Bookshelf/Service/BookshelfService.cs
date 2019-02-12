using AutoMapper;
using Kaspersky.Database;
using Kaspersky.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
