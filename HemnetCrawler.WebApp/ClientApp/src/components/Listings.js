import React, { useEffect } from 'react';
import { Link } from 'react-router-dom';
import './tables.css';
import './listingsMisc.css';
import { prettySEK } from './Utils.js';

export default function Listings() {
    const [listings, setListings] = React.useState([]);

    const [usersFilter, setFilter] = React.useState({ homeType: "All", roomsMinimum: null, roomsMaximum: null });
    const homeTypeValuesWithRooms = ['All', 'Fritidshus', 'Lägenhet', 'Villa'];
    const hasRooms = homeTypeValuesWithRooms.includes(usersFilter.homeType);

    useEffect(() => {
        fetch("/HemnetData/listings")
            .then(resp => resp.json())
            .then(data => {
                setListings(data)
            })},
        []
    );

    const filterListings = (listings) => {
/*        const hasRooms = homeTypeValuesWithRooms.includes(usersFilter.homeType);*/

        return listings
            .filter(l =>
                (usersFilter.homeType !== "All" ? l.homeType === usersFilter.homeType : true) &&
                (hasRooms && usersFilter.roomsMinimum ? parseInt(l.rooms) >= usersFilter.roomsMinimum : true) &&
                (hasRooms && usersFilter.roomsMaximum ? parseInt(l.rooms) <= usersFilter.roomsMaximum : true)
            );
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
        </tr>
    );

    const handleHomeTypeFilter = (e) => {
        setFilter({
            ...usersFilter,
            homeType: e.target.value,
            //roomsMinimum: homeTypeValuesWithRooms.includes(event.target.value) ? usersFilter.roomsMinimum : null,
            //roomsMaximum: homeTypeValuesWithRooms.includes(event.target.value) ? usersFilter.roomsMaximum : null
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

    const alternativeRoomsFilter = () => {
        if (hasRooms) {
            return (
                <div className="rooms-filters">
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
                </div>
            );
        }
    }

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
                <br></br>
                {alternativeRoomsFilter()}
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
                    </tr>
                </thead>
                <tbody>
                    {filledTableBody}
                </tbody>
             </table>
        </div>
    );
}