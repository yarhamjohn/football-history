import React, { Component } from 'react';

class ResultMatrix extends Component {
  render() {
    const {resultMatrix} = this.props;
    const teams = resultMatrix.rows.map(r => ({ homeTeam: r.homeTeam, homeTeamAbbreviation: r.homeTeamAbbreviation })).sort((a, b) => a.homeTeam.localeCompare(b.homeTeam));

    return (
      <div>
        <table className='table'>
          <thead>
            <tr>
              <th>Home / Away</th>
              {
                teams.map(t => <th key={t.homeTeam}>{t.homeTeamAbbreviation}</th>)
              }
            </tr>
          </thead>
          <tbody>
            {
              teams.map(t => {
                const row = resultMatrix.rows.filter(r => r.homeTeam === t.homeTeam)[0];
                return (
                  <tr key={t.homeTeam}>
                    <td>{t.homeTeam}</td>
                    {
                      teams.map(t => {
                        const score = row.scores.filter(s => s.awayTeam === t.homeTeam)[0];
                        return (
                          <td key={t + score.awayTeam + score.score} 
                              style={{backgroundColor: getBackgroundColor(score.result)}}
                          >
                            {score.score}
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
      </div>
    );
  }
}

function getBackgroundColor(result) {
  if (result === null) {
    return '#BBB';
  }

  return result === 'W' 
    ? '#BFB' 
    : result === 'D' 
      ? '#FFB' 
      : '#FBB'
}

export default ResultMatrix;
