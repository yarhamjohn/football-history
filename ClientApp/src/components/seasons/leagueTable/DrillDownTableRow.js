import React, { Component } from 'react';
import { LineChart } from 'react-chartkick'

import './DrillDownTableRow.css';

class DrillDownTableRow extends Component {
  constructor(props) {
    super(props);
    this.state = { form: [], positions: [], loading: true }
  }

  componentDidMount() {
    const { tier, season, team } = this.props;

    fetch(`api/FootballHistory/GetDrillDown?tier=${tier}&season=${season}&team=${team}`)
      .then(response => response.json())
      .then(data => {
        this.setState({ 
          form: data.form,
          positions: data.positions,
          loading: false 
        });
      });
  }

  render() {
    const { form, positions, loading } = this.state;
    const { numRows, relegationPosition } = this.props;

    if (loading) {
      return <tr><td colSpan="12"><em>Loading...</em></td></tr>;
    }

    const test = positions.map(p => [p.date, p.position, 'point { shape-type: star }']);

    return (
      <tr>
        <td colSpan="12">
          <div className='drilldown-form'>
            {form.map(f => <span key={f.matchDate} style={{fontWeight: 'bold', color: f.result === 'W' ? 'green' : f.result === 'D' ? 'darkorange' : 'red'}}>{f.result}</span>)}
          </div>
          <LineChart data={test} min={1} max={numRows} library={{chartArea: {width: '90%'}, vAxis: {direction: -1, baseline: relegationPosition, baselineColor: 'red' }}} />
        </td>
      </tr>
    )
  }
}

export default DrillDownTableRow;
