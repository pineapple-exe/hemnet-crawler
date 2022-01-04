import React from 'react';

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

    const scrollAndPaginate = (pageNumber) => {
        window.scrollTo(0, 0);
        props.setCurrentPageIndex(pageNumber);
    }

    const pageItemAndLink = (key, paginateTo) => {
        return (
            <>
                <li key={key} className="page-item">
                    <button onClick={() => scrollAndPaginate(paginateTo)} className={`page-link ${currentPage === key ? 'selected' : ''}`}>
                        {key}
                    </button>
                </li>
            </>
        );
    }

    const head = () => {
        const theVisibles = currentVisibilityRange();

        if (!theVisibles.includes(1)) {
            return (
                <>
                    {pageItemAndLink(1, 0)}
                    {dotdotdot(theVisibles[0] > 2)}
                </>
            );
        }
    }

    const tail = () => {
        const theVisibles = currentVisibilityRange();
        const lastPageNumber = pageNumbers[pageNumbers.length - 1];

        if (!theVisibles.includes(lastPageNumber)) {
            return (
                <>
                    {dotdotdot(theVisibles[theVisibles.length - 1] < (lastPageNumber - 1))}
                    {pageItemAndLink(lastPageNumber, lastPageNumber - 1)}
                </>
            );
        }
    }

    const abbreviatedPageItems = () => {
        return (
            <ul className="pagination">
                {head()}
                {currentVisibilityRange().map(pageNumber => (
                    pageItemAndLink(pageNumber, pageNumber - 1)
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
