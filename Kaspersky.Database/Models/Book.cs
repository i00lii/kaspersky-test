using System.Collections.Generic;

namespace Kaspersky.Database.Models
{
    public sealed class Book : DbEntity
    {
        public string Isbn { get; set; }
        public string Title { get; set; }
        public string Publisher { get; set; }

        public ushort PagesTotal { get; set; }
        public ushort? PublicationYear { get; set; }

        public ICollection<Author> Authors { get; set; }
        public ICollection<Thumbnail> Thumbnails { get; set; }
    }
}
