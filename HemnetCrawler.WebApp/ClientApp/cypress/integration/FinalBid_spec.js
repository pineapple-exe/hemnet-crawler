import data from '../fixtures/data.json';

describe('Final bids', () => {
    it('Checks if FinalBids show up', () => {
        cy.visit(data.baseUrl);

        cy.contains('Final Bids').click();

        cy.get('tr.final-bid').should('exist');
    });
});
