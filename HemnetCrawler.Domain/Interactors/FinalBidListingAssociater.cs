using HemnetCrawler.Domain.Repositories;
using HemnetCrawler.Domain.Entities;
using System.Linq;
using FuzzySharp;
using System.Collections.Generic;

namespace HemnetCrawler.Domain.Interactors
{
    public class FinalBidListingAssociater
    {
        private readonly IListingRepository _listingRepository;
        private readonly IFinalBidRepository _finalBidRepository;

        public FinalBidListingAssociater(IListingRepository listingRepository, IFinalBidRepository finalBidRepository)
        {
            _listingRepository = listingRepository;
            _finalBidRepository = finalBidRepository;
        }

        private static EvaluatedFinalBidMatch GenerateEvaluatedMatch(Listing listing, FinalBid finalBid)
        {
            static int binaryPrecisionValue(bool condition) => condition ? 1 : 0;

            double precisionRate = Fuzz.Ratio(listing.Street, finalBid.Street) / 100.0 +
                binaryPrecisionValue(listing.HomeType == finalBid.HomeType) +
                binaryPrecisionValue(listing.PostalCode == finalBid.PostalCode && listing.PostalCode != null && finalBid.PostalCode != null) +
                binaryPrecisionValue(listing.Price == finalBid.DemandedPrice) +
                binaryPrecisionValue(listing.OwnershipType == finalBid.OwnershipType) +
                binaryPrecisionValue(listing.Fee == finalBid.Fee) +
                binaryPrecisionValue(listing.Rooms == finalBid.Rooms);

            return new EvaluatedFinalBidMatch(finalBid.Id, precisionRate);
        }

        public void AlgorithmAddFinalBidsToListings()
        {
            var finalBids = _finalBidRepository.GetAll().ToList();

            foreach (Listing listing in _listingRepository.GetAllListings().Where(l => string.IsNullOrEmpty(l.Href)).ToList())
            {
                List<EvaluatedFinalBidMatch> evaluatedFinalBidMatches = new();

                foreach (FinalBid finalBid in finalBids)
                {
                    if (listing.Published < finalBid.SoldDate)
                    { 
                        EvaluatedFinalBidMatch match = GenerateEvaluatedMatch(listing, finalBid);
                        evaluatedFinalBidMatches.Add(match);
                    }
                }

                if (evaluatedFinalBidMatches.Count != 0)
                {
                    evaluatedFinalBidMatches = evaluatedFinalBidMatches.OrderBy(efb => efb.PrecisionRate).ToList();

                    int bestMatchId = evaluatedFinalBidMatches.Last().FinalBidId;

                    listing.FinalBidId = bestMatchId;

                    _listingRepository.UpdateListing(listing);

                    finalBids.RemoveAll(fb => fb.Id == bestMatchId);
                }
            }
        }
    }
}
