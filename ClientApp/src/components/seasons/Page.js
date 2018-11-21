import React, { Component } from 'react';
import Season from './Table';
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

  render() {
    let { loading, allSeasons, allTiers, selectedSeason, selectedTier } = this.state;
    
    if (loading) {
      return <p><em>Loading...</em></p>;
    }
  
    var season = selectedSeason === null ? allSeasons[0] : selectedSeason;
    var tier = selectedTier === null ? allTiers[0] : selectedTier;

    return (
      <React.Fragment>
        <Filter 
          allSeasons={allSeasons} 
          allTiers={allTiers} 
          selectedTier={tier} 
          selectedSeason={season}
          selectLeagueTable={(tier, season) => this.selectSeason(tier, season)}
        />
        <Season season={season} tier={tier} />
      </React.Fragment>
    );
  }
}

export default Page;
