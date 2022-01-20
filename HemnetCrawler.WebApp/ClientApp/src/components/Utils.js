import prettyMoney from "pretty-money";
import React from 'react';

export const prettySEK = prettyMoney({
        currency: "kr",
        maxDecimal: 0,
        thousandsDelimiter: " "
});

export const loadingScreen = (loading) => {
    if (loading) return (
        <div className="overlay">
            <div className="loader"></div>
        </div>
    );
}

export const entityProperty = (propertyValue) => {
    if (!propertyValue) {
        return (
            <span className="unknown-value">unknown</span>
        );
    } else {
        return propertyValue;
    }
}

export const tableHead = (propertyNames, reEvaluateSortDirectionBy) => {
    const clickables = (names) => (
        <>
            {names.map(name => (
                <th key={name}>
                    <button className="order-by" onClick={() => reEvaluateSortDirectionBy(name)}>
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

export const convertToFormalPropertyName = (propertyAlias) => {
    let formalPropertyName = propertyAlias[0].toUpperCase();

    for (let i = 1; i < propertyAlias.length; i++) {
        if (propertyAlias[i - 1] === ' ') {
            formalPropertyName += propertyAlias[i].toUpperCase();
        }
        else if (propertyAlias[i] === ' ') {
            continue;
        }
        else {
            formalPropertyName += propertyAlias[i];
        }
    }
    return formalPropertyName;
}