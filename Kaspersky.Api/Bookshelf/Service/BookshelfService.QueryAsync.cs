using Kaspersky.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kaspersky.Api.Bookshelf.Service
{
    public sealed partial class BookshelfService
    {
        public async Task<IReadOnlyCollection<Models.Identified<Models.Book>>> QueryAsync
        (
            Models.QueryRequestSortTargetField target,
            Models.QueryRequestSortDirection direction
        )
        {
            var items = await CreateQuery().ToArrayAsync();

            return items
                .Select
                (
                    item => new Models.Identified<Models.Book>()
                    {
                        Id = item.Id,
                        Value = _mapper.Map<Models.Book>( item )
                    }
                )
                .ToArray();

            IQueryable<Book> CreateQuery()
            {
                var query = _context
                    .Books
                    .Include( item => item.Authors )
                    .AsQueryable();

                if (target == Models.QueryRequestSortTargetField.Title)
                {
                    Expression<Func<Book, string>> propertyExpression = book => book.Title;
                    return ApplySortOrder( direction, query, propertyExpression );
                }
                else
                {
                    Expression<Func<Book, ushort?>> propertyExpression = book => book.PublicationYear;
                    return ApplySortOrder( direction, query, propertyExpression );
                }
            }
        }

        private static IOrderedQueryable<Book> ApplySortOrder<TProperty>
        (
            Models.QueryRequestSortDirection direction,
            IQueryable<Book> query,
            Expression<Func<Book, TProperty>> propertyExpression
        )
            => direction == Models.QueryRequestSortDirection.Asc
            ? query.OrderBy( propertyExpression )
            : query.OrderByDescending( propertyExpression );
    }
}
