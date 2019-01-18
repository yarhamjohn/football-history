import React, { Component } from 'react';

class ResultMatrix extends Component {
  constructor(props) {
    super(props);
    this.state = { 
      matrix: [], 
      isLoading: true 
    };
  };

  fetchResultMatrix() {
    const { tier, season } = this.props;

    fetch(`api/LeagueSeason/GetResultMatrix?tier=${tier.level}&season=${season}`)
      .then(response => response.json())
      .then(data => {
        this.setState({ 
          matrix: data,
          isLoading: false 
        });
      });
  };

  componentDidMount() {
    this.fetchResultMatrix();
  };

  componentDidUpdate(prevProps) {
    if (prevProps.tier !== this.props.tier || prevProps.season !== this.props.season) {
      this.setState({ isLoading: true }, this.fetchResultMatrix());
    }
  };

  getTeams() {
    const teams = this.state.matrix.map(r => ({ 
      homeTeam: r.homeTeam, 
      homeTeamAbbreviation: r.homeTeamAbbreviation })
    );

    return teams.sort((a, b) => a.homeTeam.localeCompare(b.homeTeam));
  }

  render() {
    const { matrix, isLoading } = this.state;

    if (isLoading) {
      return <p><em>Loading...</em></p>
    }

    const teams = this.getTeams();

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
              teams.map(t => <MatrixRow matrix={matrix} team={t} teams={teams} key={t.homeTeam} />)
            }
          </tbody>
        </table>
      </div>
    );
  }
}

function MatrixRow(props) {
    const row = props.matrix.filter(r => r.homeTeam === props.team.homeTeam)[0];
    return (
      <tr>
        <td>{props.team.homeTeam}</td>
        {
          props.teams.map(t => <MatrixCell scores={row.scores} team={t} key={t.homeTeam} />)
        }
      </tr>
    )
}

function MatrixCell(props) {
    const score = props.scores.filter(s => s.awayTeam === props.team.homeTeam)[0];
    return (
      <td style={{backgroundColor: getBackgroundColor(score.result)}}>
        {score.score}
      </td>
    )
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
