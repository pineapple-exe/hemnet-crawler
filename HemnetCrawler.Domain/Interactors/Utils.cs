using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HemnetCrawler.Domain.Interactors
{
    internal class Utils
    {
        internal static IEnumerable<TEntity> OrderByStation<TEntity, TProperty>(IQueryable<TEntity> entities, SortDirection order, Func<TEntity, TProperty> orderByRule)
        {
            CultureInfo culture = new("sv-SE");
            StringComparer stringComparer = StringComparer.Create(culture, false);
            string stringOrderByRule(TEntity e) => (string)Convert.ChangeType(orderByRule(e), typeof(string));

            if (order == SortDirection.Ascending)
            {
                if (typeof(TProperty) == typeof(string))
                    return entities.OrderBy(stringOrderByRule, stringComparer);
                else
                    return entities.OrderBy(orderByRule);
            }
            else
            {
                if (typeof(TProperty) == typeof(string))
                    return entities.OrderByDescending(stringOrderByRule, stringComparer);
                else
                    return entities.OrderByDescending(orderByRule);
            }
        }
    }
}
