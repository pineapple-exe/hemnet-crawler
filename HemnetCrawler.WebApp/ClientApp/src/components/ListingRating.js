import React, { useEffect } from 'react';
import './listingRating.css';

export function ListingRating(props) {
    const [kitchenLocal, setKitchenLocal] = React.useState(null);
    const [bathroomLocal, setBathroomLocal] = React.useState(null);

    const [kitchen, setKitchen] = React.useState(null);
    const [bathroom, setBathroom] = React.useState(null);

    const [freshlyRated, setFreshlyRated] = React.useState(false);

    useEffect(() => {
        fetch('/ListingsData/listingRating?' + new URLSearchParams({
            listingId: props.listingId
        }))
            .then(resp => resp.json())
            .then(data => {
                setKitchenLocal(data.kitchenRating)
                setBathroomLocal(data.bathroomRating)

                setKitchen(data.kitchenRating)
                setBathroom(data.bathroomRating)
            })
    }, [props.listingId]
    );

    const latestClick = (num, type) => {
        return (
            num === type ?
                'already-chosen' :
                ''
        );
    }

    const ratingValues = [{ value: 0, expression: 'Bad' }, { value: 1, expression: 'Decent' }, { value: 2, expression: 'Nice' }];

    const ratingNumber = (type, setter, ratingValue) => {
        return (
            <button
                key={ratingValue}
                className={latestClick(ratingValue, type)}
                onClick={() => {
                    setter(latestClick(ratingValue, type) === 'already-chosen' ? null : ratingValue)
                    setFreshlyRated(setFreshlyRated(false))
                }}>
                {ratingValues.find(rv => rv.value === ratingValue).expression}
            </button>
        );
    }

    const renderRatingButtons = (type, setter) => {
        let buttons = [];

        for (let i = 0; i < 3; i++) {
            buttons.push(
                ratingNumber(type, setter, i)
            );
        }
        return buttons;
    }

    const rateListing = () => {
        fetch('/ListingsData/rateListing', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ listingId: props.listingId, kitchenRating: kitchenLocal, bathroomRating: bathroomLocal })
        }).then(() => {
            setKitchen(kitchenLocal)
            setBathroom(bathroomLocal)
        });
        setFreshlyRated(true);
    }

    const ratedResponse = () => {
        if (freshlyRated) {
            return (
                <p>Thank you!</p>
            );
        }
    }

    return (
        <div className="rating">
            <div className="kitchen-rating">
                <p>Your impression of the kitchen:</p>
                {renderRatingButtons(kitchenLocal, setKitchenLocal)}
            </div>
            <div className="bathroom-rating">
                <p>Your impression of the bathroom:</p>
                {renderRatingButtons(bathroomLocal, setBathroomLocal)}
            </div>
            <button
                id="rate"
                disabled={kitchen === kitchenLocal && bathroom === bathroomLocal}
                onClick={rateListing}
            >
                Rate Listing
            </button>
            <div className="freshly-rated">
                {ratedResponse()}
            </div>
        </div>
    );
}
