﻿import data from '../fixtures/data.json';

let listingProperties = [];
const ratingTypes = ['kitchen', 'bathroom'];

const enterFirstListing = () => {
    cy.visit(`${data.baseUrl}listings`);

    cy.get('td').first().click();
}

describe('Listings', () => {
    it('Checks if Listings show up', () => {
        cy.visit(data.baseUrl);

        cy.contains('Listings').click();

        cy.get('tr.listing').should('exist');
    })
});

describe('Listing property values', () => {
    it('Checks property value-correspondence between Listing row and Listing page', () => {
        cy.visit(`${data.baseUrl}listings`);

        cy.get('tr.listing').first().children().then(tds => {
            tds.each((tdIndex, td) => {
                listingProperties.push(td.textContent === '' ? 'unknown' : td.textContent)
            });

            cy.visit(`${data.baseUrl}listing/${listingProperties[0]}`);

            cy.get('.listing-properties > li > .property-value').then(lis => {
                lis.each((liIndex, li) => {
                    expect(li.textContent).to.equal(listingProperties[liIndex])
                })
            });
        });
    });
});

describe('Listing navigation', () => {
    it('Navigates to individual Listing page', () => {
        enterFirstListing();
    })
});

describe('Listing Estimation', () => {
    it('Checks Estimation functionality', () => {
        enterFirstListing();

        cy.get('.estimation p').should('not.exist');

        cy.contains('Estimate Final Price').click();

        cy.get('.estimation p').should('exist');
    })
});

describe('Rating options', () => {
    it('Checks the existence of instructions', () => {
        enterFirstListing();

        for (let i = 0; i < ratingTypes.length; i++) {
            cy.get(`div.${ratingTypes[i]}-rating>p`).should('not.be.empty');
        }
    });

    it('Checks the existence of options', () => {
        enterFirstListing();

        for (let i = 0; i < ratingTypes.length; i++) {
            cy.get(`div.${ratingTypes[i]}-rating>button`).should('have.length', 3);
        }
    });

    it('Checks Option button pressed down functionality', () => {
        enterFirstListing();

        for (let i = 0; i < ratingTypes.length; i++) {
            cy.get(`div.${ratingTypes[i]}-rating>button`).first().click();

            cy.get(`div.${ratingTypes[i]}-rating>button`).first().should('have.class', 'already-chosen');

            cy.get(`div.${ratingTypes[i]}-rating>button`).last().click();

            cy.get(`div.${ratingTypes[i]}-rating>button`).first().should('not.have.class', 'already-chosen');
        }
    });

    it('Checks Rate Listing button responsivity', () => {
        enterFirstListing();

        cy.get(`div.${ratingTypes[0]}-rating>button`).not('.already-chosen').first().click();

        cy.get('#rate').click();

        cy.get('div.freshly-rated>p').should('exist');
    });
});