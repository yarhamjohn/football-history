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
                {leagueSeason.leagueTable.map(row => <TableRow row={row} numRows={leagueSeason.leagueTable.length} tier={leagueSeason.tier} season={leagueSeason.season} key={row.position} relegationPosition={GetRelegationPosition(leagueSeason.leagueTable)} />)}
              </tbody>
            </table>
            <div>
              {leagueSeason.leagueTable.map(row =>
                row.pointsDeducted > 0
                  ? PointsDeductionMessage(row.team, row.pointsDeducted, row.pointsDeductionReason)
                  : '')}
            </div>
          </div>
    );
  }
}

function GetRelegationPosition(leagueTable)
{
  const relegationRows = leagueTable.filter(r => r.status === 'R');
  return leagueTable.length - relegationRows.length + 1;
}

function PointsDeductionMessage(team, pointsDeducted, pointsDeductionReason)
{
  const message = `* ${team} had ${pointsDeducted} point${pointsDeducted > 1 ? 's' : ''} deducted (${pointsDeductionReason})`
  return <p key={team} className='point-deductions'>{message}</p>
}

export default Table;
