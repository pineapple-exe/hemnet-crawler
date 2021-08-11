import data from '../fixtures/data.json';

const enterFinalBidEstimation = () => {
    cy.visit(`${data.baseUrl}listings`);

    cy.get('td').first().click();

    cy.get('.estimation-link').click();
}

describe('Estimation page', () => {
    it('Enters Estimation page', () => {
        enterFinalBidEstimation();
    });

    it('Checks Estimation functionality', () => {
        enterFinalBidEstimation();

        cy.get('.estimation > p').should('not.exist');

        cy.get('.estimation > button', { timeout: 15000 }).contains('Estimate Final Price').click();

        cy.get('.estimation > p').should('exist');
    });

    it('Checks the existence of Final Bids Table', () => {
        enterFinalBidEstimation();

        cy.get('table.relevant-finalbids', { timeout: 15000 }).should('exist');
    });
});