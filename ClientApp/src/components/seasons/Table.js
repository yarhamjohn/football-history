import React, { Component } from 'react';
import TableRow from './TableRow';
import './Table.css';

class Table extends Component {
  render() {
    const { leagueSeason } = this.props;

    return (
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
                {leagueSeason.leagueTable.rows.map(row => <TableRow row={row} tier={leagueSeason.tier} season={leagueSeason.season} key={row.position} />)}
              </tbody>
            </table>
            <div>
              {leagueSeason.leagueTable.rows.map(row =>
                row.pointsDeducted > 0
                  ? PointsDeductionMessage(row.team, row.pointsDeducted, row.pointsDeductionReason)
                  : '')}
            </div>
          </div>
    );
  }
}

function PointsDeductionMessage(team, pointsDeducted, pointsDeductionReason)
{
  const message = `* ${team} had ${pointsDeducted} point${pointsDeducted > 1 ? 's' : ''} deducted (${pointsDeductionReason})`
  return <p key={team} className='point-deductions'>{message}</p>
}

export default Table;
