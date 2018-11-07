import React, { Component } from 'react';

class Table extends Component {
  render() {
    const {leagueTable} = this.props;

    return (
      <React.Fragment>
        <h1>{leagueTable.competition} </h1>
        <h2>{leagueTable.season}</h2>
        <table className='table'>
          <thead>
            <tr>
              <th>Position</th>
              <th>Team</th>
              <th>Played</th>
              <th>Won</th>
              <th>Drawn</th>
              <th>Lost</th>
              <th>Goals For</th>
              <th>Goals Against</th>
              <th>Goal Diff.</th>
              <th>Points</th>
            </tr>
          </thead>
          <tbody>
            {leagueTable.leagueTableRow.map(row =>
              <tr key={row.position}>
                <td>{row.position}</td>
                <td>{row.team}</td>
                <td>{row.played}</td>
                <td>{row.won}</td>
                <td>{row.drawn}</td>
                <td>{row.lost}</td>
                <td>{row.goalsFor}</td>
                <td>{row.goalsAgainst}</td>
                <td>{row.goalDifference}</td>
                <td>{row.points}</td>
              </tr>
            )}
          </tbody>
        </table>
      </React.Fragment>
    );
  }
}

export default Table;
