import React from 'react';
import { Link } from 'react-router-dom';
import { useEffect } from 'react';
import './tables.css';
import { prettySEK } from './Utils.js';
import Pagination from './Pagination.js';
import DeleteEntity from './DeleteEntity.js';

export default function FinalBids() {
    const [finalBids, setFinalBids] = React.useState([]);
    const [total, setTotal] = React.useState(null);
    const [loading, setLoading] = React.useState(false);
    const [currentPageIndex, setCurrentPageIndex] = React.useState(0);
    const [finalBidsPerPage] = React.useState(50);

    useEffect(() => {
        setLoading(true);
        fetch("/FinalBidsData/finalBids?" + new URLSearchParams({
            pageIndex: currentPageIndex,
            size: finalBidsPerPage
        }))
            .then(resp => resp.json())
            .then(data => {
                setFinalBids(data.items);
                setTotal(data.total);
            })
            .then(setLoading(false))
    }, [currentPageIndex, finalBidsPerPage, finalBids]
    );

    const deleteFinalBid = (finalBidId) => {
        fetch('/FinalBidsData/deleteFinalBid?' + new URLSearchParams({
            finalBidId: finalBidId
        }), {
            method: 'DELETE'
        }
        );
    }

    const filledTableBody = finalBids.map(fb =>
        <tr className="final-bid" key={fb.id}>
            <td><Link className="id" to={`/finalBid/${fb.id}`}>{fb.id}</Link></td>
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
            <td>
                <DeleteEntity id={fb.id} deleteEntity={deleteFinalBid} />
            </td>
        </tr>
    );

    const reEvaluateOrderBy = (propertyName) => {
        setOrder(by !== propertyName ? 0 : order === 0 ? 1 : 0);
        setBy(propertyName);
    }

    if (loading) {
        return (
            <p>Please wait while loading final bids...</p>
        );
    } else {
        return (
            <div>
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
                            <th>Delete</th>
                        </tr>
                    </thead>
                    <tbody>
                        {filledTableBody}
                    </tbody>
                </table>
                <Pagination entitiesPerPage={finalBidsPerPage} totalEntities={total} currentPageZeroBased={currentPageIndex} setCurrentPage={setCurrentPageIndex} />
            </div>
        );
    }
}