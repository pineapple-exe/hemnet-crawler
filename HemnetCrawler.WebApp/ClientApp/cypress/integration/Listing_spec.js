const baseUrl = 'https://localhost:44394/';
let listingProperties = [];

describe('Listings', () => {
    it('Checks if Listings show up', () => {
        cy.visit(baseUrl);

        cy.contains('Listings').click();

        cy.get('tr.listing').should('exist');
    })
});

describe('Listing property values', () => {
    it('Checks property value-correspondence between Listing row and Listing page', () => {
        cy.visit(`${baseUrl}listings`);

        cy.get('tr.listing').first().children().then(tds => {
            tds.each((tdIndex, td) => {
                listingProperties.push(td.textContent === '' ? 'unknown' : td.textContent)
            });

            cy.visit(`${baseUrl}listing/${listingProperties[0]}`);

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
        cy.visit(`${baseUrl}listings`);

        cy.get('td').first().click();
    })
});

describe('Listing Estimation', () => {
    it('Checks Estimation functionality', () => {
        cy.visit(`${baseUrl}listings`);

        cy.get('td').first().click();

        cy.get('.estimation p').should('not.exist');

        cy.contains('Estimate Final Price').click();

        cy.get('.estimation p').should('exist');
    })
});