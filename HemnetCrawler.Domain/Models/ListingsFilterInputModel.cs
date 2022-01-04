using HemnetCrawler.Domain.Entities;
using System.Linq;

namespace HemnetCrawler.Domain.Models
{
    public class ListingsFilterInputModel
    {
        public string HomeType { get; set; }
        public int? RoomsMinimum { get; set; }
        public int? RoomsMaximum { get; set; }
        public string Street { get; set; }
    }
}
