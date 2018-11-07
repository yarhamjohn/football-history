import React, { Component } from 'react';

class Table extends Component {
  constructor(props) {
    super(props);
    this.state = { leagueTable: {}, loading: true };
  }

  componentDidMount() {
    fetch('api/FootballHistory/GetLeague')
      .then(response => response.json())
      .then(data => {
        this.setState({ leagueTable: data, loading: false });
      });
  }

  render() {
    let body;

    if (this.state.loading) {
      body = <p><em>Loading...</em></p>
    } else {
      body = <React.Fragment>
        <h1>{this.state.leagueTable.competition} ({this.state.leagueTable.season})</h1>
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
            {this.state.leagueTable.leagueTableRow.map(row =>
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
    };

    return (
      <div>
        {body}
      </div>
    );
  }
}

export default Table;
