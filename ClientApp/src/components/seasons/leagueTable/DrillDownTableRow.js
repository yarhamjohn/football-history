import React, { Component } from 'react';
import { Well } from 'react-bootstrap';
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

    const test = positions.map(p => [p.date, p.position]);

    return (
      <tr>
        <td colSpan="12">
          <Well>
            <div style={{backgroundColor: 'white', padding: 5}}>
              <div className='drilldown-form'>
                {form.map(f => <span key={f.matchDate} style={{fontWeight: 'bold', color: f.result === 'W' ? 'green' : f.result === 'D' ? 'darkorange' : 'red'}}>{f.result}</span>)}
              </div>
              <LineChart 
                data={test} 
                min={1} 
                max={numRows} 
                library={{
                  chartArea: {
                    width: '90%', 
                    height: '75%'
                  }, 
                  pointSize: 0,
                  hAxis: {
                    gridlines: {
                      color: '#CCC', 
                      count: -1
                    }, 
                    format: 'MMM d, y'
                  }, 
                  vAxis: {
                    ticks: GetTicks(numRows), 
                    direction: -1, 
                    baseline: relegationPosition, 
                    baselineColor: 'red'
                  }
                }} />
            </div>
          </Well>
        </td>
      </tr>
    )
  }
}

function GetTicks(numRows)
{
  if (numRows === 20)
  {
    return [1,5,10,15,20];
  }

  return [1,6,12,18,24];
}

export default DrillDownTableRow;
