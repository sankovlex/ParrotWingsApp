using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParrotWings.Data.Mappings
{
    public interface IEntityMappingConfiguration<T> : IEntityMappingConfiguration where T : class
    {
        void Map(EntityTypeBuilder<T> builder);
    }
}
