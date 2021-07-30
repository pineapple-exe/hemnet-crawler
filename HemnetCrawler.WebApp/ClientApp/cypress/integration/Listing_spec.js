import data from '../fixtures/data.json';
let listingProperties = [];

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
        cy.visit(`${data.baseUrl}listings`);

        cy.get('td').first().click();
    })
});

describe('Listing Estimation', () => {
    it('Checks Estimation functionality', () => {
        cy.visit(`${data.baseUrl}listings`);

        cy.get('td').first().click();

        cy.get('.estimation p').should('not.exist');

        cy.contains('Estimate Final Price').click();

        cy.get('.estimation p').should('exist');
    })
});