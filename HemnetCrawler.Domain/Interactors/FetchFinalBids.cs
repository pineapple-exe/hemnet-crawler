using HemnetCrawler.Domain.Models;
using HemnetCrawler.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace HemnetCrawler.Domain.Interactors
{
    public class FetchFinalBids
    {
        private readonly IFinalBidRepository _finalBidRepository;

        public FetchFinalBids(IFinalBidRepository finalBidRepository)
        {
            _finalBidRepository = finalBidRepository;
        }

        public List<FinalBidOutputModel> ListFinalBids()
        {
            var allFinalBids = _finalBidRepository.GetAll().Take(100);

            List<FinalBidOutputModel> outputModels = allFinalBids.Select(fb => new FinalBidOutputModel
            {
                Id = fb.Id,
                Street = fb.Street,
                City = fb.City,
                PostalCode = fb.PostalCode,
                Price = fb.Price,
                SoldDate = fb.SoldDate,
                DemandedPrice = fb.DemandedPrice,
                PriceDevelopment = fb.PriceDevelopment,
                HomeType = fb.HomeType,
                Rooms = fb.Rooms,
                LivingArea = fb.LivingArea,
                Fee = fb.Fee
            }).ToList();

            return outputModels;
        }
    }
}
