import React from 'react';

export default function EntityFiltering(props) {
    const optionalRoomsFilter = () => {
        if (props.homeTypeValuesWithRooms.includes(props.usersFilter.homeType)) {
            return (
                <form className="rooms-filters">
                    <label>Minimum rooms:</label>
                    <input
                        type="number"
                        onChange={props.handleRoomsMinimumFilter}
                        min="0" max="50"
                        placeholder={props.usersFilter.roomsMinimum}
                    />

                    <label>Maximum rooms:</label>
                    <input
                        type="number"
                        onChange={props.handleRoomsMaximumFilter}
                        min="1" max="50"
                        placeholder={props.usersFilter.roomsMaximum}
                    />

                    <button className="reset" onClick={() => props.resetRoomsFilter}>Reset</button>
                </form>
            );
        }
    }

    return (
        <div className="entity-filters">
            <form>
                <label>Home type:</label>
                <select className="filter" onChange={props.handleHomeTypeFilter} >
                    <option value="All">*</option>
                    <option value="Tomt">Tomt</option>
                    <option value="Villa">Villa</option>
                    <option value="Lägenhet">Lägenhet</option>
                    <option value="Gård med jordbruk">Gård med jordbruk</option>
                </select>
            </form>

            {optionalRoomsFilter()}

            <form>
                <label>Street:</label>
                <input type="text" value={props.usersFilter.street.street} onChange={props.handleStreetFilter} />
            </form>
        </div>
    );
}