using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ParrotWings.Data.Mappings
{
    public abstract class EntityMappingConfiguration<T> : IEntityMappingConfiguration<T> where T : class
    {
        public abstract void Map(EntityTypeBuilder<T> builder);

        public void Map(ModelBuilder builder)
        {
            Map(builder.Entity<T>());
        }
    }
}
