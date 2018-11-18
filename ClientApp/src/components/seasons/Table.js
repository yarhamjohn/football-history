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

  getRowColour(status) {
    if (status === 'Champions') {
      return '#BBF3FF';
    } else if (status === 'Promoted') {
      return '#BFB';
    } else if (status === 'Relegated') {
      return '#FBB';
    } else if (status === 'Play Off Winner') {
      return '#FFE4B5';
    } else if (status === 'Play Offs') {
      return '#FFB';
    }

    return '';
  }

  render() {
    const {leagueTable, loading} = this.state;
    let body;

    if (loading) {
      body = <p><em>Loading...</em></p>
    } else {
      body = <div style={{marginRight: 100}}>
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
              <th>GF</th>
              <th>GA</th>
              <th>Diff</th>
              <th>Points</th>
              <th>Status</th>
            </tr>
          </thead>
          <tbody>
            {leagueTable.leagueTableRows.map(row =>
              <tr key={row.position} style={{backgroundColor: this.getRowColour(row.status)}}>
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
                <td>{row.status === 'Champions' ? 'C' : row.status === 'Promoted' ? 'P' : row.status === 'Play Offs' ? 'PO' : row.status === 'Play Off Winner' ? 'P (PO)' : row.status === 'Relegated' ? 'R' : ''}</td>
              </tr>
            )}
          </tbody>
        </table>
        <div>
          {leagueTable.leagueTableRows.map(row =>
            row.pointsDeducted > 0
              ? <p key={row.team} className='point-deductions'>* {row.team} had {row.pointsDeducted} point{row.pointsDeducted > 1 ? 's' : ''} deducted ({row.pointsDeductionReason})</p>
              : '')}
        </div>
      </div>
    }

    return (
      <React.Fragment>
        {body}
      </React.Fragment>
    );
  }
}

export default Table;
