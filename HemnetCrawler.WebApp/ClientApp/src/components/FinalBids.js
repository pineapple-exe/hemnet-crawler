import React from 'react';
import { useEffect } from 'react';
import './tables.css';

export default function FinalBids(props) {
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
            <td>{fb.id}</td>
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