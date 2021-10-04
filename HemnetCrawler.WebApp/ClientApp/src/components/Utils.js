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