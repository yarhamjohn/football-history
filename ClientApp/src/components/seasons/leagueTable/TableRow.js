import React, { Component } from 'react';
import { Glyphicon } from 'react-bootstrap';
import DrillDownTableRow from './DrillDownTableRow';

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

  getRowColour(status) {
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

  render() {
    const { row, tier, season, numRows, relegationPosition } = this.props;

    return (
      <React.Fragment>
        <tr
          style={{backgroundColor: this.getRowColour(row.status)}}
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
        {this.state.DrillDownShowing && <DrillDownTableRow tier={tier} season={season} team={row.team} numRows={numRows} relegationPosition={relegationPosition}/>}
      </React.Fragment>
    )
  }
}

export default TableRow;
