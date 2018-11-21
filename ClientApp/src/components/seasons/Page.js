import React, { Component } from 'react';
import Table from './Table';
import Filter from './Filter';

class Page extends Component {
  constructor(props) {
    super(props);
    this.state = { 
      allSeasons: [],
      allTiers: [],
      selectedSeason: null,
      selectedTier: null,
      loading: true };
  }

  selectSeason(tier, season) {
    this.setState({selectedSeason: season, selectedTier: tier });
  }

  componentDidMount() {
    fetch(`api/FootballHistory/GetFilterOptions`)
      .then(response => response.json())
      .then(data => {
        this.setState({ 
          allSeasons: data.allSeasons.sort().reverse(), 
          allTiers: data.allTiers.sort((a, b) => a.tier - b.tier), 
          loading: false 
        });
      });
  }

  getCompetitionName(season, tier) {
    return "Competition Name";
  }

  render() {
    let body;
    let { loading, allSeasons, allTiers, selectedSeason, selectedTier } = this.state;
    
    if (loading) {
      body = <p><em>Loading...</em></p>
    } else {
      var season = selectedSeason === null ? allSeasons[0] : selectedSeason;
      var tier = selectedTier === null ? allTiers[0] : selectedTier;

      body = <React.Fragment>
        <Filter 
          allSeasons={allSeasons} 
          allTiers={allTiers} 
          selectedTier={tier} 
          selectedSeason={season}
          selectLeagueTable={(tier, season) => {console.log("1:", tier); this.selectSeason(tier, season)}}
        />
        <h1>{this.getCompetitionName(season, tier)}</h1>
        <h2>{season}</h2>
        <Table season={season} tier={tier} />
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
