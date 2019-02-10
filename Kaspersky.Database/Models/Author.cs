using System;

namespace Kaspersky.Database.Models
{
    public sealed class Author : DbEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public Guid BookId { get; set; }
        public Book Book { get; set; }
    }
}
