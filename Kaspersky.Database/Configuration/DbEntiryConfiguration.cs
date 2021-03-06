﻿using Kaspersky.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kaspersky.Database.Configuration
{
    internal sealed class DbEntiryConfiguration<T> : IEntityTypeConfiguration<T> where T : DbEntity
    {
        public static DbEntiryConfiguration<T> Value { get; } = new DbEntiryConfiguration<T>();

        public void Configure( EntityTypeBuilder<T> builder )
            => builder.HasKey( item => item.Id );
    }
}
