import React, { useEffect } from 'react';
import './entityProfile.css';
import prettyMoney from "pretty-money";

export default function Listing(props) {
    const [listing, setListing] = React.useState(null);
    const [estimatedFinalPrice, setEstimatedFinalPrice] = React.useState(null);

    useEffect(() => {
        fetch('/hemnetData/listing?' + new URLSearchParams({
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

    const getEstimatedFinalPrice = () => {
        fetch('/hemnetData/estimatedPrice?' + new URLSearchParams({
            listingId: listing.id,
            }))
                .then(resp => resp.json())
                .then(data => { setEstimatedFinalPrice(data.price) })
    }

    const prettySEK = prettyMoney({
        currency: "kr",
        maxDecimal: 0,
        thousandsDelimiter: " ",
    });

    const estimationStatus = () => {
        if (estimatedFinalPrice) {
            return (
                <p>
                    <span className="estimation-key">Estimation: </span>
                    {prettySEK(estimatedFinalPrice)}
                </p>
            );
        } else {
            return (
                <button type="button" onClick={() => getEstimatedFinalPrice()}>
                    Estimate Final Price
                </button>
            );
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
                    src={`/hemnetData/image?imageId=${imageId}`}
                    alt="listing"
                />
            )
        );

        return (
            <div className="listing-container">
                <ul className="listing-properties">
                    <li><span className="property-name">Id:</span> {listing.id}</li>
                    <li><span className="property-name">Street:</span> {listing.street}</li>
                    <li><span className="property-name">City:</span> {listing.city}</li>
                    <li><span className="property-name">Postal code:</span> {listingProperty(listing.postalCode)}</li>
                    <li><span className="property-name">Price:</span> {listingProperty(prettySEK(listing.price))}</li>
                    <li><span className="property-name">Rooms:</span> {listing.rooms === null ? '-' : listing.rooms}</li>
                    <li><span className="property-name">Home type:</span> {listing.homeType}</li>
                    <li><span className="property-name">Living area:</span> {listing.livingArea === null ? '-' : listing.livingArea}</li>
                    <li><span className="property-name">Fee:</span> {listingProperty(prettySEK(listing.fee))}</li>
                </ul>

               <div className="estimation">
                    {estimationStatus()}
               </div>

                <div className="gallery">
                    {gallery}
                </div>
            </div>
        );
    }
}