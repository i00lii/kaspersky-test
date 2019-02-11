using FluentValidation;
using Kaspersky.Utils.Text;
using System.Collections.Generic;
using DbBook = Kaspersky.Database.Models.Book;


namespace Kaspersky.Api.Bookshelf.Models
{
    public sealed class Book
    {
        public string Title { get; set; }
        public string Publisher { get; set; }
        public string Isbn { get; set; }

        public ushort PagesTotal { get; set; }
        public ushort? PublicationYear { get; set; }

        public IReadOnlyCollection<Author> Authors { get; set; }

        public sealed class Profile : AutoMapper.Profile
        {
            public Profile()
            {
                CreateMap<Book, DbBook>();
                CreateMap<DbBook, Book>();
            }
        }

        public sealed class Validator : AbstractValidator<Book>
        {
            public Validator()
            {
                const int textThreshold = 30;

#pragma warning disable CS1587 // XML comment is not placed on a valid language element
                /// заголовок (обязательный параметр, не более 30 символов)

                RuleFor( item => item.Title )
                    .NotEmpty()
                    .MaximumLength( textThreshold );

                /// список авторов (книга должна содержать хотя бы одного автора)
                RuleFor( item => item.Authors )
                    .NotNull()
                    .Must( item => item?.Count > 0 );

                RuleForEach( item => item.Authors )
                    .NotNull()
                    .SetValidator( new Author.Validator() );

                /// количество страниц (обязательный параметр, больше 0 и не более 10000)
                RuleFor( item => item.PagesTotal )
                    .NotEmpty()
                    .LessThanOrEqualTo( (ushort)10000 );

                /// год публикации( опциональный параметр, не раньше 1800)
                RuleFor( item => item.PublicationYear )
                    .GreaterThanOrEqualTo( (ushort)1800 )
                    .When( item => item.PublicationYear.HasValue );

                /// название издательства (опциональный параметр, не более 30 символов)
                RuleFor( item => item.Publisher )
                    .NotEmpty()
                    .MaximumLength( textThreshold )
                    .When( item => item.Publisher != default );

                /// ISBN с валидацией (обязательный параметр, http://en.wikipedia.org/wiki/International_Standard_Book_Number)
                RuleFor( item => item.Isbn )
                    .NotEmpty()
                    .Must( item => item.IsValidIsbnString() );

#pragma warning restore CS1587 // XML comment is not placed on a valid language element
            }
        }
    }
}
