import React, { Component, useEffect } from 'react';
import './tables.css';

export default function Listings() {
    const [listings, setListings] = React.useState([]);

    useEffect(() => {
        fetch("/hemnetData/listings")
            .then(resp => resp.json())
            .then(data => {
                setListings(data)
            });
    });

    const filledTableBody = listings.map(l =>
        <tr key={l.id}>
            <td>{l.id}</td>
            <td>{l.street}</td>
            <td>{l.city}</td>
            <td>{l.postalCode}</td>
            <td>{l.price}</td>
            <td>{l.rooms}</td>
            <td>{l.homeType}</td>
            <td>{l.livingArea}</td>
            <td>{l.fee}</td>
        </tr>
     );

            return (
        <table>
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Street</th>
                    <th>City</th>
                    <th>Postal code</th>
                    <th>Price</th>
                    <th>Rooms</th>
                    <th>Home type</th>
                    <th>Living area</th>
                    <th>Fee</th>
                </tr>
            </thead>
            <tbody>
                {filledTableBody}
            </tbody>
        </table>
    );
}