using Kaspersky.Api.Bookshelf.Models;
using Kaspersky.Api.Bookshelf.Service;
using Kaspersky.Api.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Kaspersky.Api.Bookshelf
{
    /// <summary>
    /// Управление книгами
    /// </summary>
    [Route( "api/bookshelf" )]
    [ApiController]
    public sealed class BookshelfController : ControllerBase
    {
        private static readonly Thumbnail.Validator _thumbnailValidator = new Thumbnail.Validator();

        private readonly IBookshelfService _bookshelf;

        public BookshelfController( IBookshelfService bookshelf ) => _bookshelf = bookshelf;

        /// <summary>
        /// Добавление книги
        /// </summary>
        [HttpPost( "book/add" )]
        public Task AddAsync( Book book ) => _bookshelf.AddAsync( book );

        /// <summary>
        /// Загрузка изображения обложки
        /// </summary>
        [HttpPost( "book/{bookId}/thumbnail" )]
        public async Task<IActionResult> AddThumbnailAsync( Guid bookId, IFormFile attachment )
        {
            var bytes = await ReadBody();
            var thumbnail = new Thumbnail() { Data = bytes };
            var validationResult = _thumbnailValidator.Validate( thumbnail );

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                    ModelState.TryAddModelError( error.PropertyName, error.ErrorMessage );

                return new BadRequestObjectResult( ModelState );
            }

            await _bookshelf.AddThumbnailAsync( bookId, thumbnail );
            return Ok();

            async Task<byte[]> ReadBody()
            {
                using (var stream = attachment.OpenReadStream())
                using (var ms = new MemoryStream())
                {
                    await stream.CopyToAsync( ms );
                    ms.Position = 0;
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// Удаление по идентификатору
        /// </summary>
        [HttpGet( "book/{bookId}/delete" )]
        public Task DeleteAsync( Guid bookId ) => _bookshelf.DeleteAsync( bookId );

        /// <summary>
        /// Изменение
        /// </summary>
        [HttpPost( "book/{bookId}/update" )]
        public Task UpdateAsync( Guid bookId, Book book ) => _bookshelf.UpdateAsync( bookId, book );

        /// <summary>
        /// Получение списка с сортировкой по году публикации или заголовку
        /// </summary>
        [HttpGet( "book/search" )]
        public async Task<ApiResponse<IReadOnlyCollection<Identified<Book>>>> SearchAsync
        (
            QueryRequestSortTargetField field = QueryRequestSortTargetField.Title,
            QueryRequestSortDirection direction = QueryRequestSortDirection.Asc
        )
        {
            var items = await _bookshelf.QueryAsync( field, direction );
            return ApiResponse.Create( items );
        }
    }
}
