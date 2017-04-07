using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ParrotWings.Data.Extensions;
using ParrotWings.Models.Domain.Accounts;
using ParrotWings.Models.Domain.Transactions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ParrotWings.Data.Core
{
    public class EfContext : DbContext, IEfContext
    {
        //public EfContext()
        //{

        //}

        //public EfContext(DbContextOptions optionsBuilder) : base(optionsBuilder)
        //{
        //}

        #region Tables
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddEntityConfigurationsFromAssembly(GetType().GetTypeInfo().Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Server=(localdb)\mssqllocaldb;Database=myway_db;Trusted_Connection=True;
            //Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
            //if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=pw_db;Integrated Security=True;");
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public new EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            return base.Entry(entity);
        }

        public new int SaveChanges()
        {
            return base.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
