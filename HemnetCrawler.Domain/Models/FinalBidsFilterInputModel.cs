
namespace HemnetCrawler.Domain.Models
{
    public class FinalBidsFilterInputModel
    {
        public string HomeType { get; set; }
        public int? RoomsMinimum { get; set; }
        public int? RoomsMaximum { get; set; }
        public string Street { get; set; }
    }
}
