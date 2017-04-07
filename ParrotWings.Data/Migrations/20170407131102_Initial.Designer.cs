using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ParrotWings.Data.Core;

namespace ParrotWings.Data.Migrations
{
    [DbContext(typeof(EfContext))]
    [Migration("20170407131102_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ParrotWings.Models.Domain.Accounts.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("newsequentialid()");

                    b.Property<DateTime>("DateCreate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("Salt")
                        .IsRequired();

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Name");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ParrotWings.Models.Domain.Transactions.Transaction", b =>
                {
                    b.Property<long>("TransactionId")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<DateTime>("DateCreate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Message")
                        .HasMaxLength(50);

                    b.Property<Guid>("RecipientId");

                    b.Property<Guid>("SenderId");

                    b.HasKey("TransactionId");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SenderId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("ParrotWings.Models.Domain.Transactions.Transaction", b =>
                {
                    b.HasOne("ParrotWings.Models.Domain.Accounts.User", "Recipient")
                        .WithMany("ReceivedTransations")
                        .HasForeignKey("RecipientId");

                    b.HasOne("ParrotWings.Models.Domain.Accounts.User", "Sender")
                        .WithMany("SentTransactions")
                        .HasForeignKey("SenderId");
                });
        }
    }
}
