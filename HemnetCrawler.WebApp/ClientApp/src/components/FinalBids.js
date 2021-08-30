import React from 'react';
import { Link } from 'react-router-dom';
import { useEffect } from 'react';
import './tables.css';
import { prettySEK } from './Utils.js';

export default function FinalBids() {
    const [finalBids, setFinalBids] = React.useState([]);

    useEffect(() => {
        fetch("/hemnetData/finalBids")
            .then(resp => resp.json())
            .then(data => {
                setFinalBids(data);
            })
        }, []
    );

    const filledTableBody = finalBids.map(fb =>
        <tr className="final-bid" key={fb.id}>
            <td><Link to={`/finalBid/${fb.id}`}>{fb.id}</Link></td>
            <td>{fb.street}</td>
            <td>{fb.city}</td>
            <td>{fb.postalCode}</td>
            <td>{prettySEK(fb.price)}</td>
            <td>{fb.rooms}</td>
            <td>{fb.homeType}</td>
            <td>{fb.livingArea}</td>
            <td>{prettySEK(fb.fee)}</td>
            <td>{fb.soldDate}</td>
            <td>{prettySEK(fb.demandedPrice)}</td>
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
                    <th>Sold date</th>
                    <th>Demanded price</th>
                </tr>
            </thead>
            <tbody>
                {filledTableBody}
            </tbody>
        </table>
    );
}