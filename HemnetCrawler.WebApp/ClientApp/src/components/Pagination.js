////import React from 'react';

////export default function Pagination({ totalEntities, entitiesPerPage, paginate }) {
////    let pageNumbers = [];

////    for (let i = 1; i < Math.ceil(totalEntities / entitiesPerPage); i++) {
////        pageNumbers.push(i);
////    }

////    return (
////        <nav>
////            <ul className="pagination">
////                {pageNumbers.map(number => (
////                    <li key={number} className="page-item">
////                        <a onClick={() => paginate(number)} href="!#" className="page-link">
////                            {number}
////                        </a>
////                    </li>
////                ))}
////            </ul>
////        </nav>
////    );
////};
