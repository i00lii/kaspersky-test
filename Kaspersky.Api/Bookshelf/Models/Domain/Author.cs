using FluentValidation;
using System;
using DbAuthor = Kaspersky.Database.Models.Author;

namespace Kaspersky.Api.Bookshelf.Models
{
    public sealed class Author
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public sealed class Profile : AutoMapper.Profile
        {
            public Profile()
            {
                CreateMap<Author, DbAuthor>();
                CreateMap<DbAuthor, Author>();
            }
        }

        public class Validator : AbstractValidator<Author>
        {
            public Validator()
            {
                const int nameThreshold = 20;
                RuleFor( item => item.Name ).NotEmpty().MaximumLength( nameThreshold );
                RuleFor( item => item.Surname ).NotEmpty().MaximumLength( nameThreshold );
            }
        }
    }
}
