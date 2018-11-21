import React, { Component } from 'react';
import Table from './Table';
import Filter from './Filter';
import ResultMatrix from './ResultMatrix';
import PlayOffMatches from './PlayOffMatches';

class Page extends Component {
  constructor(props) {
    super(props);
    this.state = { 
      leagueSeason: {},
      loading: true };
  }

  fetchData(tier, season) {
    fetch(`api/FootballHistory/GetLeagueSeason?tier=${tier}&season=${season}`)
      .then(response => response.json())
      .then(data => {
        this.setState({ leagueSeason: data, loading: false });
      });
  }

  componentDidMount() {
    this.fetchData(null, null);
  }

  render() {
    let body;
    let {loading, leagueSeason} = this.state;
    
    if (loading) {
      body = <p><em>Loading...</em></p>
    } else {
      var season = leagueSeason.season;
      var tier = leagueSeason.tier;

      body = <React.Fragment>
        <Filter 
          allSeasons={leagueSeason.leagueFilterOptions.allSeasons} 
          allTiers={leagueSeason.leagueFilterOptions.allTiers} 
          selectedTier={tier} 
          selectedSeason={season}
          selectLeagueTable={(tier, season) => this.fetchData(tier, season)}
        />
        <h1>{leagueSeason.competitionName}</h1>
        <h2>{season}</h2>
        <div style={{display: 'flex', flexWrap: 'wrap'}}>
          <Table leagueTable={leagueSeason.leagueTable} />
          {tier !== 1 && <PlayOffMatches playOffs={leagueSeason.playOffs} />}
        </div>
        <ResultMatrix resultMatrix={leagueSeason.resultMatrix} />
      </React.Fragment>
    }

    return (
      <React.Fragment>
        {body}
      </React.Fragment>
    );
  }
}

export default Page;
