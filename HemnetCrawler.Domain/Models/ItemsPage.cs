using System.Collections.Generic;

namespace HemnetCrawler.Domain.Models
{
    public class ItemsPage<T>
    {
        public List<T> Items { get; }
        public int Total { get; }

        public ItemsPage(List<T> items, int total)
        {
            Items = items;
            Total = total;
        }
    }
}
