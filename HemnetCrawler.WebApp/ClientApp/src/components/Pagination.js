import React from 'react';

//export default function Pagination(props) {
//    return (
//        <p>Hiii, I'm a component! {props.totalEntities} {props.entitiesPerPage}</p>
//    );
//}

export default function Pagination(props) {
    let pageNumbers = [];
    const currentPage = props.currentPageZeroBased + 1;

    for (let i = 1; i < Math.ceil(props.totalEntities / props.entitiesPerPage); i++) {
        pageNumbers.push(i);
    }

    const currentVisibilityRange = () => {
        let range = 20; // Mandatory First and Last excluded.
        let lowerRange = range / 2;
        let upperRange = range / 2;

        const isNotFirstOrLast = (number) => {
            return number !== 1 && number !== pageNumbers.length;
        }

        const lowerNumbers = pageNumbers.filter(number =>
            isNotFirstOrLast(number) &&
            (number < currentPage) &&
            (number >= currentPage - lowerRange)
        );

        upperRange += lowerRange - lowerNumbers.length;

        const upperNumbers = pageNumbers.filter(number =>
            isNotFirstOrLast(number) &&
            (number > currentPage) &&
            (number <= currentPage + upperRange)
        );

        for (let i = lowerNumbers[0] - 1; i > 1 && ((lowerNumbers.length + upperNumbers.length) < range); i--) {
            lowerNumbers.unshift(i);
        }

        return [].concat(lowerNumbers, [currentPage], upperNumbers);
    }

    const dotdotdot = (condition) => {
        if (condition) {
            return (
                <p>...</p>
            );
        }
    }

    const head = () => {
        const theVisibles = currentVisibilityRange();

        if (!theVisibles.includes(1)) {
            return (
                <>
                    <li key={1} className="page-item">
                        <button onClick={() => scrollAndPaginate(0)} className="page-link">
                            {1}
                        </button>
                    </li>
                    {dotdotdot(theVisibles[0] > 2)}
                </>
            );
        }
    }

    const scrollAndPaginate = (pageNumber) => {
        window.scrollTo(0, 0);
        props.setCurrentPage(pageNumber);
    }

    const tail = () => {
        const theVisibles = currentVisibilityRange();
        const lastPageNumber = pageNumbers[pageNumbers.length - 1];

        if (!theVisibles.includes(lastPageNumber)) {
            return (
                <>
                    {dotdotdot(theVisibles[theVisibles.length - 1] < (lastPageNumber - 1))}
                    <li key={lastPageNumber} className="page-item">
                        <button onClick={() => scrollAndPaginate(lastPageNumber - 1)} className="page-link">
                            {lastPageNumber}
                        </button>
                    </li>
                </>
            );
        }
    }

    const abbreviatedPageItems = () => {
        return (
            <ul className="pagination">
                {head()}
                {currentVisibilityRange().map(pageNumber => (
                    <li key={pageNumber} className="page-item">
                        <button onClick={() => scrollAndPaginate(pageNumber - 1)} className="page-link">
                            {pageNumber}
                        </button>
                    </li>
                ))}
                {tail()}
            </ul>
        );
    };

    return (
        <nav>
            {abbreviatedPageItems()}
        </nav>
    );
}
