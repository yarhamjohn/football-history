import React, { Component } from 'react';
import TableRow from './TableRow';
import baseUrl from "../../../api/LeagueSeasonApi";
import './LeagueTable.css';

class LeagueTable extends Component {
  constructor(props) {
    super(props);
    this.state = { 
      table: [], 
      isLoading: true 
    };
  };

  fetchLeagueTable() {
    const { tier, seasonStartYear } = this.props;

    fetch(`${baseUrl}/api/LeagueSeason/GetLeagueTable?tier=${tier.level}&seasonStartYear=${seasonStartYear}`)
      .then(response => response.json())
      .then(data => {
        this.setState({ 
          table: data.rows,
          isLoading: false 
        });
      });
  };

  componentDidMount() {
    this.fetchLeagueTable();
  };

  componentDidUpdate(prevProps) {
    if (prevProps.tier !== this.props.tier || prevProps.seasonStartYear !== this.props.seasonStartYear) {
      this.setState({ isLoading: true }, this.fetchLeagueTable());
    }
  };

  render() {
    const { table, isLoading } = this.state;
    const { tier, seasonStartYear } = this.props;

    if (isLoading) {
      return <p><em>Loading...</em></p>
    }

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
            {
              table.map(row => 
                <TableRow 
                  row={row} 
                  numRows={table.length} 
                  tier={tier}
                  seasonStartYear={seasonStartYear} 
                  key={row.position} 
                  relegationPosition={GetRelegationPosition(table)}
                />
              )
            }
          </tbody>
        </table>
        <div>
          {table.map(row =>
            row.pointsDeducted > 0
              ? PointsDeductionMessage(row.team, row.pointsDeducted, row.pointsDeductionReason)
              : '')}
        </div>
      </div>
    );
  }
}

function GetRelegationPosition(table)
{
  const relegationRows = table.filter(r => r.status === 'R');
  return table.length - relegationRows.length + 1;
}

function PointsDeductionMessage(team, pointsDeducted, pointsDeductionReason)
{
  const message = `* ${team} had ${pointsDeducted} point${pointsDeducted > 1 ? 's' : ''} deducted (${pointsDeductionReason})`
  return <p key={team} className='point-deductions'>{message}</p>
}

export default LeagueTable;
