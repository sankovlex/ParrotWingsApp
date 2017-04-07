using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ParrotWings.Data.Extensions
{
    public static class EagerLoadingExtension
    {
        public static IIncludableQueryable<TEntity, TProperty> IncludeData<TEntity, TProperty>(this IQueryable<TEntity> query,
    Expression<Func<TEntity, TProperty>> include)
    where TEntity : class
        {
            return query.Include(include);
        }

        public static IIncludableQueryable<TEntity, TProperty> IncludeData<TEntity, TProperty>(this IIncludableQueryable<TEntity, TProperty> query,
            Expression<Func<TEntity, TProperty>> include)
            where TEntity : class
        {
            return query.Include(include);
        }

        public static IIncludableQueryable<TEntity, TProperty> ThenIncludeData<TEntity, TPreviousProperty, TProperty>(this IIncludableQueryable<TEntity, ICollection<TPreviousProperty>> query,
            Expression<Func<TPreviousProperty, TProperty>> include)
            where TEntity : class
        {
            return query.ThenInclude(include);
        }
    }
}
