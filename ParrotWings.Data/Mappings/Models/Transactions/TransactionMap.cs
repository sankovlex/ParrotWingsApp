using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParrotWings.Models.Domain.Transactions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;

namespace ParrotWings.Data.Mappings.Models.Transactions
{
    public class TransactionMap : EntityMappingConfiguration<Transaction>
    {
        public override void Map(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(p => p.TransactionId);

            //sender -> transactions
            builder.HasOne(p => p.Sender)
                .WithMany(d => d.SentTransactions)
                .HasForeignKey(k => k.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            //recipient -> transactions
            builder.HasOne(p => p.Recipient)
                .WithMany(d => d.ReceivedTransations)
                .HasForeignKey(k => k.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.Message).HasMaxLength(50);

            //date generate
            builder.Property(p => p.DateCreate).ValueGeneratedOnAdd().HasDefaultValueSql("getdate()");
        }
    }
}
