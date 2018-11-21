import React, { Component } from 'react';
import PlayOffMatches from './PlayOffMatches';
import ResultMatrix from './ResultMatrix';
import Table from './Table';
import './Table.css';

class LeagueSeason extends Component {
  constructor(props) {
    super(props);
    this.state = { leagueSeason: {}, loading: true };
  }

  fetchLeagueSeason() {
    const { season, tier } = this.props;

    fetch(`api/FootballHistory/GetLeagueSeason?tier=${tier.level}&season=${season}`)
      .then(response => response.json())
      .then(data => {
        this.setState({ leagueSeason: data, loading: false });
      });
  }

  componentDidMount() {
    this.fetchLeagueSeason();
  }

  componentDidUpdate(prevProps) {
    if (prevProps.season !== this.props.season || prevProps.tier.level !== this.props.tier.level) {
      this.fetchLeagueSeason();
    }
  }

  render() {
    const { leagueSeason, loading } = this.state;
    if (loading)
    {
      return <p><em>Loading...</em></p>;
    }

    return (
      <React.Fragment>
        <h1>{leagueSeason.competitionName}</h1>
        <h2>{leagueSeason.season}</h2>
        <div style={{display: 'flex', flexWrap: 'wrap'}}>
          <Table leagueSeason={leagueSeason} />
          {leagueSeason.tier !== 1 && <PlayOffMatches playOffs={leagueSeason.playOffs} />}
        </div>
        <ResultMatrix resultMatrix={leagueSeason.resultMatrix} />
      </React.Fragment>
    );
  }
}

export default LeagueSeason;
