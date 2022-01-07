import React from 'react';

export default function EntityFiltering(props) {
    const homeTypeValuesWithRooms = ['All', 'Fritidshus', 'Lägenhet', 'Villa'];

    const handleFilter = (filterObject) => {
        props.setFilter(filterObject);
        props.debouncedTriggerSetReload();
    }

    const handleHomeTypeFilter = (event) => {
        let refreshedFilter =
            homeTypeValuesWithRooms.includes(event.target.value) ?
                {
                    ...props.filter,
                    homeType: event.target.value,
                } :
                {
                    ...props.filter,
                    homeType: event.target.value,
                    roomsMinimum: ''
                }

        handleFilter(refreshedFilter);
    }

    const handleRoomsMinimumFilter = (event) => {
        handleFilter({
            ...props.filter,
            roomsMinimum: event.target.value,
        });
    }

    const handleRoomsMaximumFilter = (event) => {
        handleFilter({
            ...props.filter,
            roomsMaximum: event.target.value
        });
    }

    const handleStreetFilter = (event) => {
        handleFilter({
            ...props.filter,
            street: event.target.value
        });
    }

    const resetRoomsFilter = () => {
        handleFilter({
            ...props.filter,
            roomsMinimum: '',
            roomsMaximum: ''
        });
    }

    const optionalRoomsFilter = () => {
        if (homeTypeValuesWithRooms.includes(props.filter.homeType)) {
            return (
                <form className="rooms-filters">
                    <label>Minimum rooms:</label>
                    <input
                        type="number"
                        onChange={handleRoomsMinimumFilter}
                        min="0" max="50"
                        placeholder={props.filter.roomsMinimum}
                    />

                    <label>Maximum rooms:</label>
                    <input
                        type="number"
                        onChange={handleRoomsMaximumFilter}
                        min="1" max="50"
                        placeholder={props.filter.roomsMaximum}
                    />

                    <button className="reset" onClick={() => resetRoomsFilter}>Reset</button>
                </form>
            );
        }
    }

    return (
        <div className="entity-filters">
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

            {optionalRoomsFilter()}

            <form>
                <label>Street:</label>
                <input type="text" value={props.filter.street} onChange={handleStreetFilter} />
            </form>
        </div>
    );
}