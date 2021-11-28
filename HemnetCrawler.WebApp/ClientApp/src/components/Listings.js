import React, { useEffect } from 'react';
import { Link } from 'react-router-dom';
import './tables.css';
import './listingsMisc.css';
import { prettySEK } from './Utils.js';
import Pagination from './Pagination.js';
import DeleteEntity from './DeleteEntity.js';

export default function Listings() {
    const propertyNames = ['Id', 'Street', 'City', 'Postal code', 'Price', 'Rooms', 'Home type', 'Living area', 'Fee'];

    const [listings, setListings] = React.useState([]);
    const [total, setTotal] = React.useState(null);
    const [loading, setLoading] = React.useState(false);
    const [currentPageIndex, setCurrentPageIndex] = React.useState(0);
    const [listingsPerPage] = React.useState(50);
    const [order, setOrder] = React.useState(0);
    const [by, setBy] = React.useState(propertyNames[0]);

    const [usersFilter, setFilter] = React.useState({
        homeType: 'All',
        roomsMinimum: null,
        roomsMaximum: null,
        street: null,
    });
    const homeTypeValuesWithRooms = ['All', 'Fritidshus', 'Lägenhet', 'Villa'];
    const hasRooms = homeTypeValuesWithRooms.includes(usersFilter.homeType);

    useEffect(() => {
        setLoading(true);

        fetch('/ListingsData/listings?' + new URLSearchParams({
            pageIndex: currentPageIndex,
            size: listingsPerPage,
            order: order,
            by: by
        }))
            .then(resp => resp.json())
            .then(data => {
                setListings(data.items);
                setTotal(data.total);
            })
            .then(setLoading(false))
    }, [currentPageIndex, listingsPerPage, listings, order, by]
    );

    const filterListings = (listings) => {
        return listings
            .filter(l =>
                (usersFilter.homeType !== 'All' ? l.homeType === usersFilter.homeType : true) &&
                (hasRooms && usersFilter.roomsMinimum ? parseInt(l.rooms) >= usersFilter.roomsMinimum : true) &&
                (hasRooms && usersFilter.roomsMaximum ? parseInt(l.rooms) <= usersFilter.roomsMaximum : true) &&
                (usersFilter.street ? l.street.toLowerCase().includes(usersFilter.street.toLowerCase()) : true)
            );
    }

    const deleteListing = (listingId) => {
        fetch('/ListingsData/deleteListing?' + new URLSearchParams({
            listingId: listingId
        }), {
                method: 'DELETE'
            }
        );
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

    const reEvaluateOrderBy = (propertyName) => {
        setOrder(by != propertyName ? 0 : order == 0 ? 1 : 0);
        setBy(propertyName);
    }

    const tableHead = (propertyNames) => {
        const clickables = (names) => (
            <>
                {names.map(name => (
                    <th>
                        <button className="order-by" onClick={() => reEvaluateOrderBy(name)}>
                            {name}
                        </button>
                    </th>
                ))}
            </>
        );

        return (
            <thead>
                <tr>
                    {clickables(propertyNames)}
                    <th>Delete</th>
                </tr>
            </thead>
        );
    }

    if (loading) {
        return (
            <p>Please wait while loading listings...</p>
        );
    } else {
        return (
            <div>
                <form>
                    <label>Home type:</label>
                    <select className="filter" onChange={handleHomeTypeFilter} >
                        <option value="All">*</option>
                        <option value="Tomt">Tomt</option>
                        <option value="Villa">Villa</option>
                        <option value="Lägenhet">Lägenhet</option>
                    </select>
                </form>

                {alternativeRoomsFilter()}

                <form>
                    <label>Street:</label>
                    <input type="text" value={usersFilter.street} onChange={handleStreetFilter}/>
                </form>

                <table>
                    {tableHead(propertyNames)}
                    <tbody>
                        {filledTableBody}
                    </tbody>
                </table>
                <Pagination entitiesPerPage={listingsPerPage} totalEntities={total} currentPageZeroBased={currentPageIndex} setCurrentPage={setCurrentPageIndex} />
            </div>
        );
    }
}