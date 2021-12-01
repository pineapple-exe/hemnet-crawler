import React from 'react';
import { Link } from 'react-router-dom';
import { useEffect } from 'react';
import './tables.css';
import { prettySEK, tableHead } from './Utils.js';
import Pagination from './Pagination.js';
import DeleteEntity from './DeleteEntity.js';

export default function FinalBids() {
    const propertyNames = ['Id', 'Street', 'City', 'Postal code', 'Price', 'Rooms', 'Home type', 'Living area', 'Fee', 'Sold date', 'Demanded price'];

    const [finalBids, setFinalBids] = React.useState([]);
    const [total, setTotal] = React.useState(null);
    const [loading, setLoading] = React.useState(false);
    const [currentPageIndex, setCurrentPageIndex] = React.useState(0);
    const [finalBidsPerPage] = React.useState(50);
    const [sortDirection, setSortDirection] = React.useState(0);
    const [by, setBy] = React.useState(propertyNames[0]);

    const fetchFinalBids = () => {
        setLoading(true);

        fetch("/FinalBidsData/finalBids?" + new URLSearchParams({
            pageIndex: currentPageIndex,
            size: finalBidsPerPage,
            sortDirection: sortDirection,
            by: by
        }))
            .then(resp => resp.json())
            .then(data => {
                setFinalBids(data.items);
                setTotal(data.total);
            })
            .then(setLoading(false))
    }

    useEffect(() =>
        fetchFinalBids(),
        [currentPageIndex, finalBidsPerPage, sortDirection, by]
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

    const reEvaluateSortDirectionBy = (propertyName) => {
        setSortDirection(by != propertyName ? 0 : sortDirection == 0 ? 1 : 0);
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
                    {tableHead(propertyNames, reEvaluateSortDirectionBy)}
                    <tbody>
                        {filledTableBody}
                    </tbody>
                </table>
                <Pagination entitiesPerPage={finalBidsPerPage} totalEntities={total} currentPageZeroBased={currentPageIndex} setCurrentPage={setCurrentPageIndex} />
            </div>
        );
    }
}