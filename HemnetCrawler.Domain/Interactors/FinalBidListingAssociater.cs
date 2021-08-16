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
            static string makeRoomsComparable(string rooms) => rooms != null ? rooms.Replace(" rum", "") : rooms;
            static int binaryPrecisionValue(bool comparison) => comparison ? 1 : 0;

            double precisionRate = binaryPrecisionValue(listing.Published < finalBid.SoldDate) +
                Fuzz.Ratio(listing.Street, finalBid.Street) / 100 +
                binaryPrecisionValue(listing.HomeType == finalBid.HomeType) +
                binaryPrecisionValue(listing.PostalCode == finalBid.PostalCode) +
                binaryPrecisionValue(listing.Price == finalBid.DemandedPrice) +
                binaryPrecisionValue(listing.OwnershipType == finalBid.OwnershipType) +
                binaryPrecisionValue(listing.Fee == finalBid.Fee) +
                binaryPrecisionValue(makeRoomsComparable(listing.Rooms) == makeRoomsComparable(finalBid.Rooms));

            return new EvaluatedFinalBidMatch(finalBid.Id, precisionRate);
        }

        public void AddFinalBidsToListings()
        {
            var finalBids = _finalBidRepository.GetAll().OrderBy(fb => fb.SoldDate).ToList();

            foreach (Listing listing in _listingRepository.GetAllListings().ToList())
            {
                List<EvaluatedFinalBidMatch> evaluatedFinalBidMatches = new();

                foreach (FinalBid finalBid in finalBids)
                {
                    EvaluatedFinalBidMatch match = GenerateEvaluatedMatch(listing, finalBid);

                    if (!evaluatedFinalBidMatches.Any(efb => efb.PrecisionRate > match.PrecisionRate))
                    { 
                        evaluatedFinalBidMatches.Add(match);
                    }
                }

                if (evaluatedFinalBidMatches.Count != 0)
                { 
                    EvaluatedFinalBidMatch bestMatch = evaluatedFinalBidMatches
                        .Where(efb => efb.PrecisionRate == evaluatedFinalBidMatches
                        .Select(efb => efb.PrecisionRate).Max()).First();

                    listing.FinalBidId = bestMatch.FinalBidId;

                    _listingRepository.UpdateListing(listing);

                    finalBids.RemoveAll(fb => fb.Id == bestMatch.FinalBidId);
                }
            }
        }
    }
}
