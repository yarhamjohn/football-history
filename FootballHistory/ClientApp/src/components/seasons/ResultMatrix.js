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
          matrix: data.rows,
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

    return teams.sort((a, b) => a.homeTeamAbbreviation.localeCompare(b.homeTeamAbbreviation));
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
          props.teams.map(t => <MatrixCell results={row.results} team={t} key={t.homeTeam} />)
        }
      </tr>
    )
}

function MatrixCell(props) {
    const result = props.results.filter(s => s.awayTeam === props.team.homeTeam)[0];
    return (
      <td style={{backgroundColor: getBackgroundColor(result.homeScore, result.awayScore)}}>
        {result.homeScore === null || result.awayScore === null ? "" : `${result.homeScore}-${result.awayScore}`}
      </td>
    )
}

function getBackgroundColor(homeScore, awayScore) {
  if (homeScore === null || awayScore === null) {
    return '#BBB';
  }

  return homeScore > awayScore
    ? '#BFB' 
    : homeScore < awayScore
      ? '#FBB' 
      : '#FFB'
}

export default ResultMatrix;
