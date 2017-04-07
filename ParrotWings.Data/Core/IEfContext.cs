using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParrotWings.Data.Core
{
    public interface IEfContext : IDisposable
    {
        /// <summary>
        /// Serialized model from database
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        /// <summary>
        /// Change tracking for model
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Save changes in database
        /// </summary>
        /// <returns>Saving status</returns>
        int SaveChanges();
        /// <summary>
        /// ASYNC: Save changes in database
        /// </summary>
        /// <returns>Saving status</returns>
        Task<int> SaveChangesAsync();
    }
}
