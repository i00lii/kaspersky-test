using Kaspersky.Api.Bookshelf.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kaspersky.Api.Bookshelf.Service
{
    public interface IBookshelfService
    {
        Task AddAsync( Book book );
        Task DeleteAsync( Guid bookId );
        Task UpdateAsync( Guid bookId, Book book );
        Task AddThumbnailAsync( Guid bookId, Thumbnail thumbnail );
        Task<IReadOnlyCollection<Identified<Book>>> QueryAsync( QueryRequestSortTargetField field, QueryRequestSortDirection direction );
    }
}
