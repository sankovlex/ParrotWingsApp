using ParrotWings.Models.Domain.Accounts;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ParrotWings.Data.Mappings.Models.Accounts
{
    public class UserMap : EntityMappingConfiguration<User>
    {
        public override void Map(EntityTypeBuilder<User> builder)
        {
            //guid generate
            builder.Property(p => p.UserId).ValueGeneratedOnAdd().HasDefaultValueSql("newsequentialid()");
            //date generate
            builder.Property(p => p.DateCreate).ValueGeneratedOnAdd().HasDefaultValueSql("getdate()");

            //indexes
            builder.HasIndex(p => p.Name);
            builder.HasIndex(p => p.Email).IsUnique();

            //requires
            builder.Property(p => p.Email).IsRequired();
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Password).IsRequired();
            builder.Property(p => p.Salt).IsRequired();
        }
    }
}
