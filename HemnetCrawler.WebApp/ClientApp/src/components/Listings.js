import React, { useEffect } from 'react';
import { Link } from 'react-router-dom';
import './tables.css';
import './listingsMisc.css';
import { prettySEK, loadingScreen, tableHead, convertToFormalPropertyName } from './Utils.js';
import Pagination from './Pagination.js';
import DeleteEntity from './DeleteEntity.js';

export default function Listings() {
    const propertyAliases = ['Id', 'Street', 'City', 'Postal code', 'Price', 'Rooms', 'Home type', 'Living area', 'Fee'];

    const [listings, setListings] = React.useState([]);
    const [total, setTotal] = React.useState(null);
    const [loading, setLoading] = React.useState(false);
    const [currentPageIndex, setCurrentPageIndex] = React.useState(0);
    const [listingsPerPage] = React.useState(50);
    const [sortDirection, setSortDirection] = React.useState(0);
    const [orderByProperty, setOrderByProperty] = React.useState(propertyAliases[0]);

    const [usersFilter, setFilter] = React.useState({
        homeType: 'All',
        roomsMinimum: '',
        roomsMaximum: '',
        street: '',
    });
    const homeTypeValuesWithRooms = ['All', 'Fritidshus', 'Lägenhet', 'Villa'];
    const hasRooms = homeTypeValuesWithRooms.includes(usersFilter.homeType);

    const fetchListings = () => {
        setLoading(true);

        fetch('/ListingsData/listings?' + new URLSearchParams({
            pageIndex: currentPageIndex,
            size: listingsPerPage,
            sortDirection: sortDirection,
            orderByProperty: convertToFormalPropertyName(orderByProperty)
        }))
            .then(resp => resp.json())
            .then(data => {
                setListings(data.items);
                setTotal(data.total);
            })
            .then(setLoading(false));
    };

    useEffect(() =>
        fetchListings(),
        [currentPageIndex, listingsPerPage, sortDirection, orderByProperty]
    );

    const filterListings = (listings) => {
        return listings
            .filter(l =>
                (usersFilter.homeType !== 'All' ? l.homeType === usersFilter.homeType : true) &&
                (hasRooms && usersFilter.roomsMinimum !== '' ? parseInt(l.rooms) >= usersFilter.roomsMinimum : true) &&
                (hasRooms && usersFilter.roomsMaximum !== '' ? parseInt(l.rooms) <= usersFilter.roomsMaximum : true) &&
                (usersFilter.street !== '' ? l.street.toLowerCase().includes(usersFilter.street.toLowerCase()) : true)
            );
    }

    const deleteListing = (listingId) => {
        fetch('/ListingsData/deleteListing?' + new URLSearchParams({
            listingId: listingId
        }), {
                method: 'DELETE'
        }
        ).then(() => fetchListings());
    }

    const filledTableBody = filterListings(listings).map(l =>
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

    const handleHomeTypeFilter = (event) => {
        setFilter({
            ...usersFilter,
            homeType: event.target.value,
        });
    }

    const handleRoomsMinimumFilter = (event) => {
        setFilter({
            ...usersFilter,
            roomsMinimum: event.target.value,
        });
    }

    const handleRoomsMaximumFilter = (event) => {
        setFilter({
            ...usersFilter,
            roomsMaximum: event.target.value
        });
    }

    const handleStreetFilter = (event) => {
        setFilter({
            ...usersFilter,
            street: event.target.value
        });
        console.log(event.target.value);
    }

    const alternativeRoomsFilter = () => {
        if (hasRooms) {
            return (
                <form className="rooms-filters">
                    <label>Minimum rooms:</label>
                    <input
                        type="number"
                        onChange={handleRoomsMinimumFilter}
                        min="1" max="50"
                        placeholder={usersFilter.roomsMinimum}
                    />

                    <label>Maximum rooms:</label>
                    <input
                        type="number"
                        onChange={handleRoomsMaximumFilter}
                        min="1" max="50"
                        placeholder={usersFilter.roomsMaximum}
                    />
                </form>
            );
        }
    }

    const reEvaluateSortDirectionBy = (propertyName) => {
        setSortDirection(orderByProperty !== propertyName ? 0 : sortDirection === 0 ? 1 : 0);
        setOrderByProperty(propertyName);
    }

    return (
        <div className="listings table-container">
            {loadingScreen(loading)}
                <form>
                    <label>Home type:</label>
                    <select className="filter" onChange={handleHomeTypeFilter} >
                        <option value="All">*</option>
                        <option value="Tomt">Tomt</option>
                        <option value="Villa">Villa</option>
                        <option value="Lägenhet">Lägenhet</option>
                        <option value="Gård med jordbruk">Gård med jordbruk</option>
                    </select>
                </form>

                {alternativeRoomsFilter()}

                <form>
                    <label>Street:</label>
                    <input type="text" value={usersFilter.street} onChange={handleStreetFilter}/>
                </form>

                <table>
                    {tableHead(propertyAliases, reEvaluateSortDirectionBy)}
                    <tbody>
                        {filledTableBody}
                    </tbody>
                </table>
                <Pagination entitiesPerPage={listingsPerPage} totalEntities={total} currentPageZeroBased={currentPageIndex} setCurrentPage={setCurrentPageIndex} />
            </div>
    );
}