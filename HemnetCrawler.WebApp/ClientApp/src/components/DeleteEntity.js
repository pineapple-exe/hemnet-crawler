import React, { useEffect } from 'react';

export default function DeleteEntity() {
    const [deletionProcess, setDeletionProcess] = React.useState({ deletePending: false, id: null });

    const deletePush = (id) => {
        if (deletionProcess.deletePending) {
            deleteListing(listingId);
            setDeletionProcess({ 'deletePending': false, 'id': null });
        } else {
            setDeletionProcess({ 'deletePending': true, 'id': id });
        }
    }

    const deleteEntity = (type, id) => {
        fetch('/ListingsData/deleteListing?' + new URLSearchParams({
            listingId: listingId
        }), {
            method: 'DELETE'
        }
        );
    }

    const deletionInitiatedX = (id) => {
        if (deletionProcess.deletePending && deletionProcess.id === id) {
            return (
                <button onClick={() => setDeletionProcess({ 'deletePending': false, 'id': null })}>
                    <h3>X</h3>
                </button>
            );
        }
    }

    const createDeleteElements = (id) => {
        return (
            <div className="delete">
                <button onClick={() => deletePush(id)}>
                    <img src="/img/trash-can.png" alt="trash-bin" />
                </button>
                {deletionInitiatedX(id)}
            </div>
        );
    }
}
