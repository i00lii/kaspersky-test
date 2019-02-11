using FluentValidation;
using System;
using DbThumbnail = Kaspersky.Database.Models.Thumbnail;

namespace Kaspersky.Api.Bookshelf.Models
{
    public sealed class Thumbnail
    {
        public byte[] Data { get; set; }

        public sealed class Profile : AutoMapper.Profile
        {
            public Profile()
            {
                CreateMap<Thumbnail, DbThumbnail>();
                CreateMap<DbThumbnail, Thumbnail>();
            }
        }

        public class Validator : AbstractValidator<Thumbnail>
        {
            public Validator() => RuleFor( item => item.Data ).NotEmpty().Must( item => item?.Length > 0 );
        }
    }
}
