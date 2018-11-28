import React, { Component } from 'react';
import './DrillDownTableRow.css';

class DrillDownTableRow extends Component {
  constructor(props) {
    super(props);
    this.state = { form: [], loading: true }
  }

  componentDidMount() {
    const { tier, season, team } = this.props;

    fetch(`api/FootballHistory/GetLeagueForm?tier=${tier}&season=${season}&team=${team}`)
      .then(response => response.json())
      .then(data => {
        this.setState({ 
          form: data,
          loading: false 
        });
      });
  }

  render() {
    const { form, loading } = this.state;

    if (loading) {
      return <tr><td colSpan="12"><em>Loading...</em></td></tr>;
    }

    return (
      <tr>
        <td colSpan="12">
          <div className='drilldown-form'>
            {form.map(f => <span key={f.matchDate} style={{fontWeight: 'bold', color: f.result === 'W' ? 'green' : f.result === 'D' ? 'darkorange' : 'red'}}>{f.result}</span>)}
          </div>
        </td>
      </tr>
    )
  }
}

export default DrillDownTableRow;
