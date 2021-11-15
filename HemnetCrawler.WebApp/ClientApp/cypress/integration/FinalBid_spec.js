import data from '../fixtures/data.json';

describe('Final bids', () => {
    it('Checks if FinalBids show up', () => {
        cy.visit(data.baseUrl);

        cy.contains('Final Bids').click();

        cy.get('tr.final-bid').should('exist');
    });
});

describe('Initiation and cancellation', () => {
    it('Confirms if you can initiate and cancel deletion of FinalBid', () => {
        cy.visit(data.baseUrl);

        cy.contains('Final Bids').click();

        cy.get('button.delete').first().should('exist');

        cy.get('button.cancel').should('not.exist');

        cy.get('button.delete').first().click({ multiple: false }).then(() => {
            cy.get('button.delete').first().should('exist');

            cy.get('button.cancel').first().should('exist');

            cy.get('button.cancel').first().click().then(() => {
                cy.get('button.delete').first().should('exist');

                cy.get('button.cancel').should('not.exist');
            });
        });
    });
});

describe('Delete Final bids', () => {
    it('Checks if FinalBid is removed from table', () => {
        cy.visit(data.baseUrl);

        cy.contains('Final Bids').click();

        cy.get('a.id').first()
            .invoke('text')
            .then((text1) => {
                cy.get('button.delete').first().click();
                cy.get('button.delete').first().click();

                cy.get('a.id').first().then(obj => {
                    if (obj.length > 0) {
                        cy.get('a.id')
                            .invoke('text')
                            .should((text2) => {
                                expect(text1).not.to.eq(text2)
                            });
                    }
                });
            });
    });
});