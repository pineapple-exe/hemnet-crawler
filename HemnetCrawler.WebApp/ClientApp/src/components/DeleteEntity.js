import React from 'react';

export default function DeleteEntity(props) {
    const [deletePending, setDeletePending] = React.useState(false);

    const deletePush = () => {
        if (deletePending) {
            props.deleteEntity(props.id);
            setDeletePending(false);
        } else {
            setDeletePending(true);
        }
    }

    const deleteInitiatedX = () => {
        if (deletePending) {
            return (
                <button className="cancel" onClick={() => setDeletePending(false)}>
                    <h3>X</h3>
                </button>
            );
        }
    }

    return (
        <div className="delete">
            <button className="delete" onClick={() => deletePush()}>
                <img src="/img/trash-can.png" alt="trash-bin" />
            </button>
            {deleteInitiatedX()}
        </div>
    );
}
