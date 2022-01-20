import React, { useEffect } from 'react';
import { Link } from 'react-router-dom';
import './tables.css';
import './listingsMisc.css';
import { prettySEK, loadingScreen, tableHead, convertToFormalPropertyName } from './Utils.js';
import Pagination from './Pagination.js';
import DeleteEntity from './DeleteEntity.js';
import _ from 'lodash';
import EntityFiltering from './EntityFiltering.js';

export default function Listings() {
    const propertyAliases = ['Id', 'Street', 'City', 'Postal code', 'Price', 'Rooms', 'Home type', 'Living area', 'Fee'];

    const [listings, setListings] = React.useState([]);
    const [total, setTotal] = React.useState(null);
    const [loading, setLoading] = React.useState(false);
    const [currentPageIndex, setCurrentPageIndex] = React.useState(0);
    const [listingsPerPage] = React.useState(50);
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

    const fetchListings = () => {
        console.log(reload);
        if (reload) {
            setLoading(true);

            fetch('/ListingsData/listings?' + new URLSearchParams({
                pageIndex: currentPageIndex,
                size: listingsPerPage,
                sortDirection: sortDirection,
                orderByProperty: convertToFormalPropertyName(orderByProperty),
                homeType: usersFilter.homeType === 'All' ? '' : usersFilter.homeType,
                roomsMinimum: usersFilter.roomsMinimum,
                roomsMaximum: usersFilter.roomsMaximum,
                street: usersFilter.street
            }))
                .then(resp => resp.json())
                .then(data => {
                    setListings(data.items);
                    setTotal(data.total);
                })
                .then(setLoading(false))
                    .then(setReload(false))
        }
    };

    useEffect(() =>
        fetchListings(),
        [currentPageIndex, listingsPerPage, sortDirection, orderByProperty, reload]
    );

    const deleteListing = (listingId) => {
        fetch('/ListingsData/deleteListing?' + new URLSearchParams({
            listingId: listingId
        }), {
                method: 'DELETE'
        }
        ).then(() => fetchListings());
    }

    const filledTableBody = listings.map(l =>
        <tr className="listing" key={l.id}>
            <td><Link className="id" to={`/listing/${l.id}`}>{l.id}</Link></td>
            <td>{l.street}</td>
            <td>{l.city}</td>
            <td>{l.postalCode}</td>
            <td>{prettySEK(l.price)}</td>
            <td>{l.rooms}</td>
            <td>{l.homeType}</td>
            <td>{l.livingArea}</td>
            <td>{prettySEK(l.fee)}</td>
            <td>
                <DeleteEntity id={l.id} deleteEntity={deleteListing} />
            </td>
        </tr>
    );

    const reEvaluateSortDirectionBy = (propertyName) => {
        setSortDirection(orderByProperty !== propertyName ? 0 : sortDirection === 0 ? 1 : 0);
        setOrderByProperty(propertyName);
        setReload(true);
    }

    return (
        <div className="listings table-container">
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
                entitiesPerPage={listingsPerPage}
                totalEntities={total}
                currentPageZeroBased={currentPageIndex}
                setCurrentPageIndex={setCurrentPageIndex}
                setReload={setReload}
            />
        </div>
    );
}