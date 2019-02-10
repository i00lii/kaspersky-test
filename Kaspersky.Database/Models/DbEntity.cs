using System;

namespace Kaspersky.Database.Models
{
    public abstract class DbEntity
    {
        public Guid Id { get; set; }
    }
}
