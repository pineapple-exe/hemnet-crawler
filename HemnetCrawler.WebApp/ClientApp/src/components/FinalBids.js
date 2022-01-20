import React from 'react';
import { Link } from 'react-router-dom';
import { useEffect } from 'react';
import './tables.css';
import { prettySEK, loadingScreen, tableHead, convertToFormalPropertyName } from './Utils.js';
import Pagination from './Pagination.js';
import DeleteEntity from './DeleteEntity.js';
import _ from 'lodash';
import EntityFiltering from './EntityFiltering.js';

export default function FinalBids() {
    const propertyAliases = ['Id', 'Street', 'City', 'Postal code', 'Price', 'Rooms', 'Home type', 'Living area', 'Fee', 'Sold date', 'Demanded price'];

    const [finalBids, setFinalBids] = React.useState([]);
    const [total, setTotal] = React.useState(null);
    const [loading, setLoading] = React.useState(false);
    const [currentPageIndex, setCurrentPageIndex] = React.useState(0);
    const [finalBidsPerPage] = React.useState(50);
    const [sortDirection, setSortDirection] = React.useState(0);
    const [orderByProperty, setOrderByProperty] = React.useState(propertyAliases[0]);
    const [reload, setReload] = React.useState(true);
    const debouncedTriggerSetReload = React.useCallback(_.debounce(() => setReload(true), 1000), [setReload]);

    const [usersFilter, setFilter] = React.useState({
        homeType: 'All',
        roomsMinimum: '',
        roomsMaximum: '',
        street: ''
    });

    const fetchFinalBids = () => {
        if (reload) {
            setLoading(true);

            fetch("/FinalBidsData/finalBids?" + new URLSearchParams({
                pageIndex: currentPageIndex,
                size: finalBidsPerPage,
                sortDirection: sortDirection,
                orderByProperty: convertToFormalPropertyName(orderByProperty),
                homeType: usersFilter.homeType === 'All' ? '' : usersFilter.homeType,
                roomsMinimum: usersFilter.roomsMinimum,
                roomsMaximum: usersFilter.roomsMaximum,
                street: usersFilter.street
            }))
                .then(resp => resp.json())
                .then(data => {
                    setFinalBids(data.items);
                    setTotal(data.total);
                })
                .then(setLoading(false))
                .then(setReload(false))
        }
    }

    useEffect(() =>
        fetchFinalBids(),
        [currentPageIndex, finalBidsPerPage, sortDirection, orderByProperty, reload]
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

            <EntityFiltering
                filter={usersFilter}
                setFilter={setFilter}
                debouncedTriggerSetReload={debouncedTriggerSetReload}
            />

            <table>
                {tableHead(propertyAliases, reEvaluateSortDirectionBy)}
                <tbody>
                    {filledTableBody}
                </tbody>
            </table>

            <Pagination
                entitiesPerPage={finalBidsPerPage}
                totalEntities={total}
                currentPageZeroBased={currentPageIndex}
                setCurrentPageIndex={setCurrentPageIndex}
                setReload={setReload}
            />
        </div>
    );
}