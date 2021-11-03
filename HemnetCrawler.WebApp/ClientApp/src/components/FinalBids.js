import React from 'react';
import { Link } from 'react-router-dom';
import { useEffect } from 'react';
import './tables.css';
import { prettySEK } from './Utils.js';
import Pagination from './Pagination.js';

export default function FinalBids() {
    const [finalBids, setFinalBids] = React.useState([]);
    const [total, setTotal] = React.useState(null);
    const [loading, setLoading] = React.useState(false);
    const [currentPage, setCurrentPage] = React.useState(0);
    const [finalBidsPerPage] = React.useState(50);
    const [deletionProcess, setDeletionProcess] = React.useState({ deletePending: false, finalBid: null });

    useEffect(() => {
        setLoading(true);
        fetch("/FinalBidsData/finalBids?" + new URLSearchParams({
            page: currentPage,
            size: finalBidsPerPage
        }))
            .then(resp => resp.json())
            .then(data => {
                setFinalBids(data.subset);
                setTotal(data.total);
            })
            .then(setLoading(false))
    }, [currentPage, finalBidsPerPage, deletionProcess]
    );

    const deletePush = (finalBidId) => {
        if (deletionProcess.deletePending) {
            deleteFinalBid(finalBidId);
            setDeletionProcess({ 'deletePending': false, 'finalBid': null });
        } else {
            setDeletionProcess({ 'deletePending': true, 'finalBid': finalBidId });
        }
    }

    const deleteFinalBid = (finalBidId) => {
        fetch('/FinalBidsData/deleteFinalBid?' + new URLSearchParams({
            finalBidId: finalBidId
        }), {
            method: 'DELETE'
        }
        );
    }

    const deletionInitiatedX = (finalBidId) => {
        if (deletionProcess.deletePending && deletionProcess.finalBid === finalBidId) {
            return (
                <button onClick={() => setDeletionProcess({ 'deletePending': false, 'finalBid': null })}>
                    <h3>X</h3>
                </button>
            );
        }
    }

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
            <td>
                <div className="delete">
                    <button onClick={() => deletePush(fb.id)}>
                        <img src="/img/trash-can.png" alt="trash-bin" />
                    </button>
                    {deletionInitiatedX(fb.id)}
                </div>
            </td>
        </tr>
    );

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
                <Pagination entitiesPerPage={finalBidsPerPage} totalEntities={total} currentPageZeroBased={currentPage} setCurrentPage={setCurrentPage} />
            </div>
        );
    }
}