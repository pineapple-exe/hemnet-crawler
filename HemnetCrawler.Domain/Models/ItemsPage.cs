using System.Collections.Generic;
using System.Linq;

namespace HemnetCrawler.Domain.Models
{
    public class ItemsPage<T>
    {
        public List<T> Items { get; }
        public int Total { get; }

        public ItemsPage(IEnumerable<T> items, int total)
        {
            Items = items.ToList();
            Total = total;
        }
    }
}
