import React, { useEffect } from 'react';
import { Link } from 'react-router-dom';
import './entityProfile.css';
import { prettySEK } from './Utils.js';
import { ListingRating } from './ListingRating.js';

export default function Listing(props) {
    const [listing, setListing] = React.useState(null);

    useEffect(() => {
        fetch('/HemnetData/listing?' + new URLSearchParams({
            listingId: props.match.params.id,
        }))
            .then(resp => resp.json())
            .then(data => { setListing(data) })},
        [props.match.params.id]
    );

    const listingProperty = (propertyValue) => {
        if (propertyValue === null) {
            return (
                <span className="unknown-value">unknown</span>
            );
        } else {
            return propertyValue;
        }
    }

    if (listing === null) {
        return (
            <h3>Please wait while loading Listing...</h3>
        );
    } else {
        const gallery = listing.imageIds.map(imageId => (
                <img
                    key={imageId}
                    src={`/HemnetData/image?imageId=${imageId}`}
                    alt="listing"
                />
            )
        );

        return (
            <div className="listing-container">
                <ul className="listing-properties">
                    <li><span className="property-name">Id:</span> <span className="property-value">{listing.id}</span></li>
                    <li><span className="property-name">Street:</span> <span className="property-value">{listing.street}</span></li>
                    <li><span className="property-name">City:</span> <span className="property-value">{listing.city}</span></li>
                    <li><span className="property-name">Postal code:</span> <span className="property-value">{listingProperty(listing.postalCode)}</span></li>
                    <li><span className="property-name">Price:</span> <span className="property-value">{listingProperty(prettySEK(listing.price))}</span></li>
                    <li><span className="property-name">Rooms:</span> <span className="property-value">{listing.rooms === null ? '-' : listing.rooms}</span></li>
                    <li><span className="property-name">Home type:</span> <span className="property-value">{listing.homeType}</span></li>
                    <li><span className="property-name">Living area:</span> <span className="property-value">{listing.livingArea === null ? '-' : listing.livingArea}</span></li>
                    <li><span className="property-name">Fee:</span> <span className="property-value">{listingProperty(prettySEK(listing.fee))}</span></li>
                </ul>

                <div className="estimation">
                    <Link to={`/finalBidEstimation/${listing.id}`}>
                        Estimate Final Price
                    </Link>
                </div>

                <div className="gallery">
                    {gallery}
                </div>

                <ListingRating listingId={listing.id} />
            </div>
            
        );
    }
}