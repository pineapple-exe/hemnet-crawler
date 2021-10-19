import React, { useEffect } from 'react';
import { Link } from 'react-router-dom';
import './tables.css';
import './listingsMisc.css';
import { prettySEK } from './Utils.js';
import Pagination from './Pagination.js';

export default function Listings() {
    const [listings, setListings] = React.useState([]);
    const [total, setTotal] = React.useState(null);
    const [loading, setLoading] = React.useState(false);
    const [currentPage, setCurrentPage] = React.useState(0);
    const [listingsPerPage] = React.useState(50);
    const [deletionProcess, setDeletionProcess] = React.useState({ 'step': 0, 'listing': null });

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
            page: currentPage,
            size: listingsPerPage
        }))
            .then(resp => resp.json())
            .then(data => {
                setListings(data.subset);
                setTotal(data.total);
            })
            .then(setLoading(false))
    }, [currentPage, listingsPerPage, deletionProcess]
    );

    const filterListings = (listings) => {
        return listings
            .filter(l =>
                (usersFilter.homeType !== 'All' ? l.homeType === usersFilter.homeType : true) &&
                (hasRooms && usersFilter.roomsMinimum ? parseInt(l.rooms) >= usersFilter.roomsMinimum : true) &&
                (hasRooms && usersFilter.roomsMaximum ? parseInt(l.rooms) <= usersFilter.roomsMaximum : true) &&
                (usersFilter.street ? l.street.includes(usersFilter.street) : true)
            );
    }

    const deletePush = (listingId) => {
        if (deletionProcess.step === 1) {
            deleteListing(listingId);
            setDeletionProcess({ 'step': 0, 'listing': null });
        } else {
            setDeletionProcess({ 'step': deletionProcess.step + 1, 'listing': listingId });
        }
    }

    const deleteListing = (listingId) => {
        fetch('/ListingsData/deleteListing?' + new URLSearchParams({
            listingId: listingId
        }), {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(listingId)
            }
        );
    }

    const deletionInitiatedX = (listingId) => {
        if (deletionProcess.step === 1 && deletionProcess.listing === listingId) {
            return (
                <button onClick={() => setDeletionProcess({'step': 0, 'listing': null})}>
                    <h3>X</h3>
                </button>
            );
        }
    }

    const filledTableBody = filterListings(listings).map(l =>
        <tr className="listing" key={l.id}>
            <td><Link to={`/listing/${l.id}`}>{l.id}</Link></td>
            <td>{l.street}</td>
            <td>{l.city}</td>
            <td>{l.postalCode}</td>
            <td>{prettySEK(l.price)}</td>
            <td>{l.rooms}</td>
            <td>{l.homeType}</td>
            <td>{l.livingArea}</td>
            <td>{prettySEK(l.fee)}</td>
            <td>
                <div className="delete">
                    <button onClick={() => deletePush(l.id)}>
                        <img src="/img/trash-can.png" alt="trash-bin" />
                    </button>

                    {deletionInitiatedX(l.id)}
                </div>
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
                            <th>Delete</th>
                        </tr>
                    </thead>
                    <tbody>
                        {filledTableBody}
                    </tbody>
                </table>
                <Pagination entitiesPerPage={listingsPerPage} totalEntities={total} currentPageZeroBased={currentPage} setCurrentPage={setCurrentPage} />
            </div>
        );
    }
}