import React, { useEffect } from 'react';
import { Link } from 'react-router-dom';
import './entityProfile.css';
import { prettySEK, entityProperty } from './Utils.js';
import { ListingRating } from './ListingRating.js';

export default function Listing(props) {
    const [listing, setListing] = React.useState(null);

    useEffect(() => {
        fetch('/ListingsData/listing?' + new URLSearchParams({
            listingId: props.match.params.id,
        }))
            .then(resp => resp.json())
            .then(data => setListing(data))
        }, [props.match.params.id]
    );

    if (!listing) {
        return (
            <h3>Please wait while loading listing...</h3>
        );
    } else {
        const gallery = listing.imageIds.map(imageId => (
                <img
                    key={imageId}
                    src={`/ListingsData/image?imageId=${imageId}`}
                    alt="listing"
                />
            )
        );

        const finalBidLinkProperty = (finalBidId) => {
            if (finalBidId) {
                return (
                    <li>
                        <span className="property-name">Final bid:</span>
                        <span className="property-value">
                            <Link to={`/finalBid/${finalBidId}`}>
                                {finalBidId}
                            </Link>
                        </span>
                    </li>
                );
            }
        }

        return (
            <div className="listing-container">
                <ul className="listing-properties">
                    <li><span className="property-name">Id:</span> <span className="property-value">{listing.id}</span></li>
                    {finalBidLinkProperty(listing.finalBidId)}
                    <li><span className="property-name">Street:</span> <span className="property-value">{listing.street}</span></li>
                    <li><span className="property-name">City:</span> <span className="property-value">{listing.city}</span></li>
                    <li><span className="property-name">Postal code:</span> <span className="property-value">{entityProperty(listing.postalCode)}</span></li>
                    <li><span className="property-name">Price:</span> <span className="property-value">{entityProperty(prettySEK(listing.price))}</span></li>
                    <li><span className="property-name">Rooms:</span> <span className="property-value">{listing.rooms === null ? '-' : listing.rooms}</span></li>
                    <li><span className="property-name">Home type:</span> <span className="property-value">{listing.homeType}</span></li>
                    <li><span className="property-name">Living area:</span> <span className="property-value">{listing.livingArea === null ? '-' : listing.livingArea}</span></li>
                    <li><span className="property-name">Fee:</span> <span className="property-value">{entityProperty(prettySEK(listing.fee))}</span></li>
                </ul>

                <Link className="estimation-link" to={`/finalBidEstimation/${listing.id}`}>
                    Estimation Page
                </Link>

                <div className="gallery">
                    {gallery}
                </div>

                <ListingRating listingId={listing.id} />
            </div>
        );
    }
}