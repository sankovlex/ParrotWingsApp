using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParrotWings.Data.Mappings
{
    public interface IEntityMappingConfiguration
    {
        void Map(ModelBuilder builder);
    }
}
