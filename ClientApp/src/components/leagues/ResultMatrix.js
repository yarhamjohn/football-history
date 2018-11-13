import React, { Component } from 'react';

class ResultMatrix extends Component {
  constructor(props) {
    super(props);
    this.state = { resultMatrix: [], loading: true };
  }

  fetchResultMatrix() {
    const { season, tier } = this.props;

    fetch(`api/FootballHistory/GetMatchResultMatrix?tier=${tier}&season=${season}`)
      .then(response => response.json())
      .then(data => {
        this.setState({ resultMatrix: data, loading: false });
      });
  }

  componentDidMount() {
    this.fetchResultMatrix();
  }

  componentDidUpdate(prevProps) {
    if (prevProps.season !== this.props.season || prevProps.tier !== this.props.tier) {
      this.fetchResultMatrix();
    }
  }

  getBackgroundColor(result) {
    if (result === null) {
      return '#BBB';
    }

    return result === 'W' 
      ? '#BFB' 
      : result === 'D' 
        ? '#FFB' 
        : '#FBB'
  }

  render() {
    const {resultMatrix, loading} = this.state;
    let body;

    const teams = resultMatrix.map(r => r.homeTeam).sort();

    if (loading) {
      body = <p><em>Loading...</em></p>
    } else {
      body = <React.Fragment>
        <table className='table'>
          <thead>
            <tr>
              <th>Home / Away</th>
              {
                teams.map(t => <th key={t}>{t}</th>)
              }
            </tr>
          </thead>
          <tbody>
            {
              teams.map(t => {
                const row = resultMatrix.filter(r => r.homeTeam === t)[0];
                return (
                  <tr key={t}>
                    <td>{t}</td>
                    {
                      teams.map(t => {
                        const score = row.scores.filter(s => s.item1 === t)[0];
                        return (
                          <td key={t + score.item1 + score.item2} 
                              style={{backgroundColor: this.getBackgroundColor(score.item3)}}
                          >
                            {score.item2}
                          </td>
                        )
                      })
                    }
                  </tr>
                )
              })
            }
          </tbody>
        </table>
      </React.Fragment>
    }

    return (
      <React.Fragment>
        {body}
      </React.Fragment>
    );
  }
}

export default ResultMatrix;
