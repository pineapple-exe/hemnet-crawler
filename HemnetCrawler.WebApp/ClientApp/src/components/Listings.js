import React, { useEffect } from 'react';
import { Link } from 'react-router-dom';
import './tables.css';
import { prettySEK } from './Utils.js';

export default function Listings() {
    const [listings, setListings] = React.useState([]);

    useEffect(() => {
        fetch("/HemnetData/listings")
            .then(resp => resp.json())
            .then(data => {
                setListings(data)
            })}, 
        []
    );

    const filledTableBody = listings.map(l =>
        <tr className="listing" key={l.id}>
            <td><Link to={`/listing/${l.id}`}>{l.id}</Link></td>
            <td>{l.street}</td>
            <td>{l.city}</td>
            <td>{l.postalCode}</td>
            <td>{prettySEK(l.price)}</td>
            <td>{l.rooms}</td>
            <td>{l.homeType}</td>
            <td>{l.livingArea}</td>
            <td>{prettySEK(l.fee)}</td>
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