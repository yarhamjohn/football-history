import React, { Component } from 'react';
import { Glyphicon } from 'react-bootstrap';
import './Table.css';

class Table extends Component {
  render() {
    const {leagueTable} = this.props;

    return (
      <React.Fragment>
        <div className='league-table'>
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
              {leagueTable.rows.map(row => <TableRow row={row} season={leagueTable.season} tier={leagueTable.tier} key={row.position} drillDown={row.form} />)}
            </tbody>
          </table>
          <div>
            {leagueTable.rows.map(row =>
              row.pointsDeducted > 0
                ? PointsDeductionMessage(row.team, row.pointsDeducted, row.pointsDeductionReason)
                : '')}
          </div>
        </div>
      </React.Fragment>
    );
  }
}

function getRowColour(status) {
  if (status === 'C') {
    return '#BBF3FF';
  } else if (status === 'P') {
    return '#BFB';
  } else if (status === 'R') {
    return '#FBB';
  } else if (status === 'PO (P)') {
    return '#FFE4B5';
  } else if (status === 'PO') {
    return '#FFB';
  }

  return '';
}

class TableRow extends Component {
  constructor(props) {
    super(props);
    this.state = {DrillDownShowing: false};
  }

  toggleDrillDown() {
    this.setState((prevProps) => {return {DrillDownShowing: !prevProps.DrillDownShowing}});
  }


  componentDidUpdate(prevProps) {
    if (prevProps.drillDown !== this.props.drillDown || prevProps.row !== this.props.row) {
      this.setState({ DrillDownShowing: false });
    }
  }
  
  render() {
    const { row, drillDown } = this.props;

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
          <td>{row.status}</td>
        </tr>
        {this.state.DrillDownShowing && <DrillDownRow drillDown={drillDown} />}
      </React.Fragment>
    )
  }
}

class DrillDownRow extends Component {
  render() {
    const { drillDown } = this.props;

    return (
      <tr>
        <td colSpan="12"><div style={{display: 'flex', justifyContent: 'space-around'}}>
          {drillDown.map(m => <span key={m.matchDate} style={{fontWeight: 'bold', color: m.result === 'W' ? 'green' : m.result === 'D' ? 'darkorange' : 'red'}}>{m.result}</span>)}
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
