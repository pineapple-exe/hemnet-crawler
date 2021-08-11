import prettyMoney from "pretty-money";

export const prettySEK = prettyMoney({
        currency: "kr",
        maxDecimal: 0,
        thousandsDelimiter: " "
});