import React from 'react';
import { Link } from 'react-router-dom';
import { useEffect } from 'react';
import './tables.css';
import { prettySEK, loadingScreen, tableHead, convertToFormalPropertyName } from './Utils.js';
import Pagination from './Pagination.js';
import DeleteEntity from './DeleteEntity.js';

export default function FinalBids() {
    const propertyAliases = ['Id', 'Street', 'City', 'Postal code', 'Price', 'Rooms', 'Home type', 'Living area', 'Fee', 'Sold date', 'Demanded price'];

    const [finalBids, setFinalBids] = React.useState([]);
    const [total, setTotal] = React.useState(null);
    const [loading, setLoading] = React.useState(false);
    const [currentPageIndex, setCurrentPageIndex] = React.useState(0);
    const [finalBidsPerPage] = React.useState(50);
    const [sortDirection, setSortDirection] = React.useState(0);
    const [orderByProperty, setOrderByProperty] = React.useState(propertyAliases[0]);

    const fetchFinalBids = () => {
        setLoading(true);

        fetch("/FinalBidsData/finalBids?" + new URLSearchParams({
            pageIndex: currentPageIndex,
            size: finalBidsPerPage,
            sortDirection: sortDirection,
            orderByProperty: convertToFormalPropertyName(orderByProperty)
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
        [currentPageIndex, finalBidsPerPage, sortDirection, orderByProperty]
    );

    const deleteFinalBid = (finalBidId) => {
        fetch('/FinalBidsData/deleteFinalBid?' + new URLSearchParams({
            finalBidId: finalBidId
        }), {
            method: 'DELETE'
        }
        ).then(() => fetchFinalBids());
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
        setSortDirection(orderByProperty !== propertyName ? 0 : sortDirection === 0 ? 1 : 0);
        setOrderByProperty(propertyName);
    }
    return (
        <div className="finalbids table-container">
            {loadingScreen(loading)}
            <table>
                {tableHead(propertyAliases, reEvaluateSortDirectionBy)}
                <tbody>
                    {filledTableBody}
                </tbody>
            </table>
            <Pagination entitiesPerPage={finalBidsPerPage} totalEntities={total} currentPageZeroBased={currentPageIndex} setCurrentPage={setCurrentPageIndex} />
        </div>
    );
}