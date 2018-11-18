import React, { Component } from 'react';
import { Glyphicon } from 'react-bootstrap';
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
      body = <div className='league-table'>
        <h1>{leagueTable.competition}</h1>
        <h2>{leagueTable.season}</h2>
        <table className='table'>
          <thead>
            <tr>
              <th></th>
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
            {leagueTable.leagueTableRows.map(row => <TableRow row={row} season={leagueTable.season} tier={this.props.tier} key={row.position}/>)}
          </tbody>
        </table>
        <div>
          {leagueTable.leagueTableRows.map(row =>
            row.pointsDeducted > 0
              ? PointsDeductionMessage(row.team, row.pointsDeducted, row.pointsDeductionReason)
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

function getRowColour(status) {
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

function getAbbreviation(status) {
  if (status === 'Champions') {
    return 'C';
  } else if (status === 'Promoted') {
    return 'P';
  } else if (status === 'Relegated') {
    return 'R';
  } else if (status === 'Play Off Winner') {
    return 'P (PO)';
  } else if (status === 'Play Offs') {
    return 'PO';
  }

  return '';
}

class TableRow extends Component 
{
  constructor(props) {
    super(props);
    this.state = {DrillDownShowing: false};
  }
  
  componentDidUpdate(prevProps) {
    if (prevProps.season !== this.props.season || prevProps.tier !== this.props.tier) {
      this.setState({DrillDownShowing: false});
    }
  }

  toggleDrillDown() {
    this.setState((prevProps) => {return {DrillDownShowing: !prevProps.DrillDownShowing}});
  }
  
  render() {
    const { row, season, tier } = this.props;

    return (
      <React.Fragment>
        <tr
          style={{backgroundColor: getRowColour(row.status)}}
          className='league-table--row'
          onClick={() => this.toggleDrillDown()}
        >
          <td><Glyphicon glyph={this.state.DrillDownShowing ? 'chevron-down' : 'chevron-right'} /></td>
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
          <td>{getAbbreviation(row.status)}</td>
        </tr>
        {this.state.DrillDownShowing && <DrillDownRow season={season} tier={tier} team={row.team} />}
      </React.Fragment>
    )
  }
}

class DrillDownRow extends Component {
  constructor(props) {
    super(props);
    this.state = { leagueTableDrillDown: [], loading: true };
  }

  fetchLeagueTableDrillDown() {
    const { season, tier, team } = this.props;

    fetch(`api/FootballHistory/GetLeagueTableDrillDown?tier=${tier}&season=${season}&team=${team}`)
      .then(response => response.json())
      .then(data => {
        this.setState({ leagueTableDrillDown: data, loading: false });
      });
  }

  componentDidMount() {
    this.fetchLeagueTableDrillDown();
  }

  render() {
    return (
      <tr>
        <td colSpan="12"><div style={{display: 'flex', justifyContent: 'space-around'}}>
          {this.state.leagueTableDrillDown.map(m => <span key={m.matchDate} style={{fontWeight: 'bold', color: m.result === 'W' ? 'green' : m.result === 'D' ? 'darkorange' : 'red'}}>{m.result}</span>)}
        </div></td>
      </tr>
    )
  }
}

function PointsDeductionMessage(team, pointsDeducted, pointsDeductionReason)
{
  const message = `* ${team} had ${pointsDeducted} point${pointsDeducted > 1 ? 's' : ''} deducted (${pointsDeductionReason})`
  return <p key={team} className='point-deductions'>{message}</p>
}

export default Table;
