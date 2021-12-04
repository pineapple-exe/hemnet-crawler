using System;
using System.Linq;
using System.Linq.Expressions;

namespace HemnetCrawler.Domain.Interactors
{
    internal static class Utils
    {
        //internal static IEnumerable<TEntity> OrderByStation<TEntity, TProperty>(IQueryable<TEntity> entities, SortDirection order, Func<TEntity, TProperty> orderByRule)
        //{
        //    CultureInfo culture = new("sv-SE");
        //    StringComparer stringComparer = StringComparer.Create(culture, false);
        //    string stringOrderByRule(TEntity e) => (string)Convert.ChangeType(orderByRule(e), typeof(string));

        //    if (order == SortDirection.Ascending)
        //    {
        //        if (typeof(TProperty) == typeof(string))
        //            return entities.OrderBy(stringOrderByRule, stringComparer);
        //        else
        //            return entities.OrderBy(orderByRule);
        //    }
        //    else
        //    {
        //        if (typeof(TProperty) == typeof(string))
        //            return entities.OrderByDescending(stringOrderByRule, stringComparer);
        //        else
        //            return entities.OrderByDescending(orderByRule);
        //    }
        //}

        internal static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, SortDirection sortDirection, string orderByProperty)
        {
            string command = sortDirection == SortDirection.Ascending ? "OrderBy" : "OrderByDescending";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                          source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }
    }
}
