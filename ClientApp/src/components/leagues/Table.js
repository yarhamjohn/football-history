import React, { Component } from 'react';
import './Table.css';

class Table extends Component {
  constructor(props) {
    super(props);
    this.state = { leagueTable: {}, loading: true };
  }

  fetchLeagueTable() {
    const { season, tier } = this.props;

    fetch(`api/FootballHistory/GetLeague?tier=${tier}&season=${season}`)
      .then(response => response.json())
      .then(data => {
        this.setState({ leagueTable: data, loading: false });
      });
  }

  componentDidMount() {
    this.fetchLeagueTable();
  }

  componentDidUpdate(prevProps) {
    if (prevProps.season !== this.props.season || prevProps.tier !== this.props.tier) {
      this.fetchLeagueTable();
    }
  }

  render() {
    const {leagueTable, loading} = this.state;
    let body;

    if (loading) {
      body = <p><em>Loading...</em></p>
    } else {
      body = <React.Fragment>
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
              <th>Goal Difference</th>
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
                <td>{row.points}{row.pointsDeducted > 0 ? <span className='point-deductions'> *</span> : ''}</td>
              </tr>
            )}
          </tbody>
        </table>
        <div>
          {leagueTable.leagueTableRow.map(row =>
            row.pointsDeducted > 0
              ? <p key={row.team} className='point-deductions'>* {row.team} had {row.pointsDeducted} point{row.pointsDeducted > 1 ? 's' : ''} deducted ({row.pointsDeductionReason})</p>
              : '')}
        </div>
      </React.Fragment>
    }

    return (
      <React.Fragment>
        {body}
      </React.Fragment>
    );
  }
}

export default Table;
