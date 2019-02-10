using System;

namespace Kaspersky.Database.Models
{
    public sealed class Thumbnail : DbEntity
    {
        public byte[] Data { get; set; }

        public Guid BookId { get; set; }
        public Book Book { get; set; }
    }
}
