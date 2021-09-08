import React from 'react';
import { useEffect } from 'react';
import { Link } from 'react-router-dom';
import './entityProfile.css';
import { prettySEK, entityProperty } from './Utils.js';

export default function FinalBid(props) {
    const [finalBid, setFinalBid] = React.useState(null);

    useEffect(() => {
        fetch('/HemnetData/finalBid?' + new URLSearchParams({
            finalBidId: props.match.params.id,
            }))
            .then(resp => resp.json())
            .then(data => setFinalBid(data))
    }, [props.match.params.id]
    );

    if (!finalBid) {
        return (
            <h3>Please wait while loading Final bid...</h3>
        );
    } else {
        const listingLinkProperty = (listingId) => {
            if (listingId) {
                return (
                    <li>
                        <span className="property-name">Listing:</span>
                        <span className="property-value">
                            <Link to={`/listing/${listingId}`}>
                                {listingId}
                            </Link>
                        </span>
                    </li>
                );
            }
        }

        return (
            <ul className="finalbid-properties">
                <li><span className="property-name">Id:</span> <span className="property-value">{finalBid.id}</span></li>
                {listingLinkProperty(finalBid.listingId)}
                <li><span className="property-name">Street:</span> <span className="property-value">{finalBid.street}</span></li>
                <li><span className="property-name">City:</span> <span className="property-value">{finalBid.city}</span></li>
                <li><span className="property-name">Postal code:</span> <span className="property-value">{entityProperty(finalBid.postalCode)}</span></li>
                <li><span className="property-name">Price:</span> <span className="property-value">{prettySEK(finalBid.price)}</span></li>
                <li><span className="property-name">Sold date:</span> <span className="property-value">{finalBid.soldDate}</span></li>
                <li><span className="property-name">Demanded price:</span> <span className="property-value">{prettySEK(finalBid.demandedPrice)}</span></li>
                <li><span className="property-name">Price development:</span> <span className="property-value">{finalBid.priceDevelopment}</span></li>
                <li><span className="property-name">Home type:</span> <span className="property-value">{finalBid.homeType}</span></li>
                <li><span className="property-name">Rooms:</span> <span className="property-value">{entityProperty(finalBid.rooms)}</span></li>
                <li><span className="property-name">Living area:</span> <span className="property-value">{entityProperty(finalBid.livingArea)}</span></li>
                <li><span className="property-name">Fee:</span> <span className="property-value">{entityProperty(finalBid.fee)}</span></li>
            </ul>
        );
    }
}


