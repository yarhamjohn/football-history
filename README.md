# Football History
This is an application for reviewing historical data from the top 4 flights of English football. It consists of a graphical dashboard and a REST API documented via a Swagger UI. The front-end is a React UI built in Typescript whilst the backend is written in C# and backed by a Sql Server database.

| Football History | Build Status |
|------------------|------------- |
| **[UI](https://football-history.azurewebsites.net/ "Football History")** | [![Build Status](https://dev.azure.com/yarhamjohn/Football%20History/_apis/build/status/Football%20History%20UI?branchName=main)](https://dev.azure.com/yarhamjohn/Football%20History/_build/latest?definitionId=1&branchName=main) |
| **[API](https://football-history-api.azurewebsites.net/swagger/index.html "Football History API")** | [![Build Status](https://dev.azure.com/yarhamjohn/Football%20History/_apis/build/status/Football%20History%20API?branchName=main)](https://dev.azure.com/yarhamjohn/Football%20History/_build/latest?definitionId=2&branchName=main) |


## Project status
Currently the data only covers the period 1992 - present, and therefore the only clubs that can be analysed are those that appeared at least once in the top 4 divisions since 1992-1993. There are 3 pages in the application:

#### Introduction
A page with a brief outline of the history of the English football league since 1888, covering major rule changes and changes to the league structure.

#### Club
A page allowing data for individual clubs to be examined. Data on this page includes the relevant league table for a given season and the historical positioning of the club across the 4 leagues.

#### League
A page allowing data for specific league seasons (e.g. 2010-2011 Premier League) to be examined. Data on this page includes the relevant league table and match results for the selected season and division.


## Planned future work
- Extend the available data back to 1888/89
- Add more metrics (e.g. trophies, records)
- Add additional analyses such as head-to-head matches
- Extend to include cup data and other leagues
- Include more details match-by-match data
