import React, { Component } from 'react';
import Table from './Table';
import Filter from './Filter';
import ResultMatrix from './ResultMatrix';
import PlayOffMatches from './PlayOffMatches';

class Page extends Component {
  constructor(props) {
    super(props);
    this.state = { 
      allSeasons: [],
      allTiers: [],
      selectedTier: null,
      selectedSeason: null,
      loading: true };
  }

  componentDidMount() {
    fetch(`api/FootballHistory/GetLeagueFilterOptions`)
      .then(response => response.json())
      .then(data => {
        this.setState({ 
          allSeasons: data.allSeasons.sort().reverse(), 
          allTiers: data.allTiers.sort((a, b) => a.tier - b.tier), 
          loading: false });
      });
  }

  selectLeagueTable(season, tier) {
    this.setState({selectedSeason: season, selectedTier: tier });
  }

  render() {
    let body;
    let {loading, allSeasons, allTiers, selectedTier, selectedSeason} = this.state;

    if (loading) {
      body = <p><em>Loading...</em></p>
    } else {
      var season = selectedSeason === null ? allSeasons[0] : selectedSeason;
      var tier = selectedTier === null ? allTiers[0].level : selectedTier;

      body = <React.Fragment>
        <Filter 
          allSeasons={allSeasons} 
          allTiers={allTiers} 
          selectedTier={tier} 
          selectedSeason={season}
          selectLeagueTable={(season, tier) => this.selectLeagueTable(season, tier)}
        />
        <div style={{display: 'flex', flexWrap: 'wrap'}}>
          <Table season={season} tier={tier} />
          {tier !== 1 && <PlayOffMatches season={season} tier={tier} />}
        </div>
        <ResultMatrix season={season} tier={tier} />
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
