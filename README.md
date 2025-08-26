# scbt-summer-practice
It is required to implement a system for currency conversion and displaying a list of exchange rates. In a simplified version, the user can view the exchange rates, convert the currency for a specified amount into another one. In a more complex version, the user can select an exchange point on the card and view the exchange rates there.


# Functional requirements
- integration is required to obtain up–to-date exchange
- rates the exchange rates must be updated once an hour
- the frontend and backend parts must be linked over HTTP (target version 1.1, the use of others is not prohibited)
- the frontend must be adaptive for mobile devices
- the frontend can be further integrated into existing websites/applications
- in addition to the current one currency exchange rate, it is required to display a chart with price changes over the last 1d/1h/1m/6m/1g
- for the backend, it is required to provide swagger documentation on endpoints for the frontend

# Technical stack of the case
Angular for the front, C# for the backend, PostgreSQL as the database.
