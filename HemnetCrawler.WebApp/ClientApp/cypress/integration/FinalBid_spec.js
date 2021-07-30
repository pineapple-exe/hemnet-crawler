const baseUrl = 'https://localhost:44394/';

describe('Final bids', () => {
    it('Checks if FinalBids show up', () => {
        cy.visit(baseUrl);

        cy.contains('Final Bids').click();

        cy.get('tr.final-bid').should('exist');
    });
});
