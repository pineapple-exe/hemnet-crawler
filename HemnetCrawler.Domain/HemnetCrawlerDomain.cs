using System;
using System.Collections.Generic;
using System.Text;
using HemnetCrawler.Domain.Repositories;

namespace HemnetCrawler.Domain
{
    public class HemnetCrawlerDomain
    {
        private IListingRepository _listingRepository;
        private IFinalBidRepository _finalBidRepository;

        public HemnetCrawlerDomain(IListingRepository listingRepository, IFinalBidRepository finalBidRepository)
        {
            _listingRepository = listingRepository;
            _finalBidRepository = finalBidRepository;
        }
    }
}
