import prettyMoney from "pretty-money";
import React from 'react';

export const prettySEK = prettyMoney({
        currency: "kr",
        maxDecimal: 0,
        thousandsDelimiter: " "
});

export const entityProperty = (propertyValue) => {
    if (!propertyValue) {
        return (
            <span className="unknown-value">unknown</span>
        );
    } else {
        return propertyValue;
    }
}

export const tableHead = (propertyNames, reEvaluateOrderBy) => {
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