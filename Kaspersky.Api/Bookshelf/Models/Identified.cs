using System;

namespace Kaspersky.Api.Bookshelf.Models
{
    public class Identified<T>
    {
        public Guid Id { get; set; }
        public T Value { get; set; }
    }
}
