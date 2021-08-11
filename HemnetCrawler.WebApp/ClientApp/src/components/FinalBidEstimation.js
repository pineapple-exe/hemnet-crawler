import React, { useEffect } from 'react';
import { Link } from 'react-router-dom';
import { prettySEK } from './Utils.js';

export default function FinalBidEstimation(props) {
    const [finalBids, setFinalBids] = React.useState(null);
    const [estimatedFinalPrice, setEstimatedFinalPrice] = React.useState(null);

    useEffect(() => {
        fetch('/HemnetData/relevantFinalBids?' + new URLSearchParams({
            listingId: props.match.params.id,
        }))
            .then(resp => resp.json())
            .then(data => { setFinalBids(data.finalBids) });
    }, [props.match.params.id]);

    const getEstimatedFinalPrice = () => {
        fetch('/HemnetData/estimatedPrice?' + new URLSearchParams({
            listingId: props.match.params.id,
        }))
            .then(resp => resp.json())
            .then(data => { setEstimatedFinalPrice(data.price) })
    }

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
                <button type="button" onClick={getEstimatedFinalPrice}>
                    Estimate Final Price
                </button>
            );
        }
    }

    if (finalBids === null) {
        return (
            <h3>Please wait while loading relevant Final Bids...</h3>
        );
    } else {
        const listingLink = (finalBid) => {
            if (finalBid.listingId != null) {
                return (
                    <Link to={`/listing/${finalBid.listingId}`}>
                        {finalBid.listingId}
                    </Link>
                );
            } else {
                return 'None';
            }
        }

        const filledTableBody = finalBids.map(fb =>
            <tr className="final-bid" key={fb.id}>
                <td>{listingLink(fb)}</td>
                <td>{fb.hasRating}</td>
                <td>{fb.street}</td>
                <td>{fb.city}</td>
                <td>{fb.postalCode}</td>
                <td>{fb.price}</td>
                <td>{fb.rooms}</td>
                <td>{fb.homeType}</td>
                <td>{fb.livingArea}</td>
                <td>{fb.fee}</td>
                <td>{fb.soldDate}</td>
                <td>{fb.demandedPrice}</td>
            </tr>
        );

        return (
            <div>
                <div className="estimation">
                    {estimationStatus()}
                </div>
                <table className="relevant-finalbids">
                    <thead>
                        <tr>
                            <th>Listing</th>
                            <th>Has rating</th>
                            <th>Street</th>
                            <th>City</th>
                            <th>Postal code</th>
                            <th>Price</th>
                            <th>Rooms</th>
                            <th>Home type</th>
                            <th>Living area</th>
                            <th>Fee</th>
                            <th>Sold date</th>
                            <th>Demanded price</th>
                        </tr>
                    </thead>
                    <tbody>
                        {filledTableBody}
                    </tbody>
                </table>
            </div>
        );
    }
}

