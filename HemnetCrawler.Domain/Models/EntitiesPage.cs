using System.Collections.Generic;

namespace HemnetCrawler.Domain.Models
{
    public class EntitiesPage<T>
    {
        public List<T> Subset { get; }
        public int Total { get; }

        public EntitiesPage(List<T> subset, int total)
        {
            Subset = subset;
            Total = total;
        }
    }
}
