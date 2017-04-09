using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ParrotWings.Data.Core.Repository
{
    public class DefaultRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private IEfContext context;
        private DbSet<TEntity> dbset;

        public DefaultRepository(IEfContext context)
        {
            this.context = context;
            dbset = context.Set<TEntity>();
        }

        private DbSet<TEntity> Entities => dbset ?? context.Set<TEntity>();

        #region CREATE, UPDATE, DELETE
        public void Create(TEntity entity)
        {
            Entities.Add(entity);
            context.SaveChanges();
        }

        public void Create(IEnumerable<TEntity> entities)
        {
            Entities.AddRange(entities);
            context.SaveChanges();
        }

        public async Task CreateAsync(TEntity entity)
        {
            Entities.Add(entity);
            await context.SaveChangesAsync();
        }

        public async Task CreateAsync(IEnumerable<TEntity> entities)
        {
            Entities.AddRange(entities);
            await context.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            Entities.Update(entity);
            context.SaveChanges();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            Entities.Update(entity);
            await context.SaveChangesAsync();
        }

        public void Delete(TEntity entity)
        {
            Entities.Remove(entity);
            context.SaveChanges();
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            Entities.RemoveRange(entities);
            context.SaveChanges();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            Entities.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            Entities.RemoveRange(entities);
            await context.SaveChangesAsync();
        }
        #endregion

        #region Retrive

        public IEnumerable<TResult> Get<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> query = null)
        {
            //if (query == null)
            //    return Entities.ToList();
            return query(Entities).ToList();
        }

        public async Task<IEnumerable<TResult>> GetAsync<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> query = null)
        {
            //if (query == null)
            //    return await Entities.ToListAsync();
            return await query(Entities).ToListAsync();
        }

        public TEntity Find(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IQueryable<TEntity>> prequery = null)
        {
            if (prequery == null)
                return Entities.FirstOrDefault(where);
            return prequery(Entities).FirstOrDefault(where);
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IQueryable<TEntity>> prequery = null)
        {
            if (prequery == null)
                return await Entities.FirstOrDefaultAsync(where);
            return await prequery(Entities).FirstOrDefaultAsync(where);
        }

        public TEntity Single(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IQueryable<TEntity>> prequery = null)
        {
            if (prequery == null)
                return Entities.SingleOrDefault(where);
            return prequery(Entities).SingleOrDefault(where);
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IQueryable<TEntity>> prequery = null)
        {
            if (prequery == null)
                return await Entities.SingleOrDefaultAsync(where);
            return await prequery(Entities).SingleOrDefaultAsync(where);
        }

        public IEnumerable<TEntity> GetSql(string sp, params object[] param)
        {
            return Entities.FromSql(sp, param).ToList();
        }

        public TEntity FindSql(string sp, params object[] param)
        {
            return Entities.FromSql(sp, param).Single();
        }

        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Entities.CountAsync(predicate);
        }
        
        public async Task<decimal> GetSumAsync<TResult>(Expression<Func<TEntity, decimal>> selector)
        {
            return await Entities.SumAsync(selector);
        }

        public int GetCount(Expression<Func<TEntity, bool>> predicate)
        {
            return Entities.Count(predicate);
        }

        public decimal GetSum<TResult>(Expression<Func<TEntity, decimal>> selector)
        {
            return Entities.Sum(selector);
        }

        #endregion
    }
}
