using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ParrotWings.Data.Core.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        #region SF/SP
        TEntity FindSql(string sp, params object[] param);
        IEnumerable<TEntity> GetSql(string sp, params object[] param);
        #endregion

        #region RETRIVE DATA
        /// <summary>
        /// Return elements of a sequence satisfy condition
        /// </summary>
        /// <typeparam name="TResult">Type of result</typeparam>
        /// <param name="query">Predicate</param>
        /// <returns></returns>
        IEnumerable<TResult> Get<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> query = null);
        /// <summary>
        /// ASYNC: Return elements of a sequence satisfy condition
        /// </summary>
        /// <param name="query">Type of result</param>
        /// <returns></returns>
        Task<IEnumerable<TResult>> GetAsync<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> query = null);
        /// <summary>
        /// Return element satisfy condition
        /// </summary>
        /// <param name="where">Predicate</param>
        /// <param name="prequery">Condition sequence</param>
        /// <returns></returns>

        int GetCount(Expression<Func<TEntity, bool>> predicate);
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate);
        decimal GetSum<TResult>(Expression<Func<TEntity, decimal>> selector);
        Task<decimal> GetSumAsync<TResult>(Expression<Func<TEntity, decimal>> selector);

        TEntity Find(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IQueryable<TEntity>> prequery = null);
        /// <summary>
        /// Return element satisfy condition
        /// </summary>
        /// <param name="where">Predicate</param>
        /// <param name="prequery">Condition sequence</param>
        /// <returns></returns>
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IQueryable<TEntity>> prequery = null);
        /// <summary>
        /// Return the only element of a sequence satisfy condition, and throwns exception if there is not exactly one element of a sequence
        /// </summary>
        /// <param name="where">Predicate</param>
        /// <param name="prequery">Condition sequence</param>
        /// <returns></returns>
        TEntity Single(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IQueryable<TEntity>> prequery = null);
        /// <summary>
        /// ASYNC: Return the only element of a sequence satisfy condition, and throwns exception if there is not exactly one element of a sequence
        /// </summary>
        /// <param name="where">Predicate</param>
        /// <param name="prequery">Condition sequence</param>
        /// <returns></returns>
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> where, Func<IQueryable<TEntity>, IQueryable<TEntity>> prequery = null);
        #endregion

        #region CREATE, UPDATE, DELETE
        /// <summary>
        /// Create entity
        /// </summary>
        /// <param name="entity"></param>
        void Create(TEntity entity);
        /// <summary>
        /// ASYNC: Create entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task CreateAsync(TEntity entity);
        /// <summary>
        /// Create sequence entity
        /// </summary>
        /// <param name="entities"></param>
        void Create(IEnumerable<TEntity> entities);
        /// <summary>
        /// ASYNC: Create sequence entity
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task CreateAsync(IEnumerable<TEntity> entities);
        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);
        /// <summary>
        /// ASYNC: Update entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateAsync(TEntity entity);
        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);
        /// <summary>
        /// ASYNC: Delete entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task DeleteAsync(TEntity entity);
        /// <summary>
        /// Delete sequence entity
        /// </summary>
        /// <param name="entities"></param>
        void Delete(IEnumerable<TEntity> entities);
        /// <summary>
        /// Delete sequence entity
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task DeleteAsync(IEnumerable<TEntity> entities);
        #endregion
    }
}
