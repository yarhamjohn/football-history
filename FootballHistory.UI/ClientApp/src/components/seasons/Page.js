import React, { Component } from 'react';
import Filter from './Filter';
import LeagueTable from './leagueTable/LeagueTable';
import PlayOffMatches from './PlayOffMatches';
import ResultMatrix from './ResultMatrix';
import baseUrl from "../../api/LeagueSeasonApi";
import './Page.css';

class Page extends Component {
  constructor(props) {
    super(props);
    this.state = { 
      allTiers: [],
      allSeasons: [],
      selectedTier: null,
      selectedSeason: null,
      isLoading: true 
    };
  };

  updateFilter(selectedTier, selectedSeason) {
    this.setState({ 
      selectedTier: selectedTier, 
      selectedSeason: selectedSeason
    });
  };

  componentDidMount() {
    fetch(`${baseUrl}/api/LeagueSeason/GetLeagueSeasonFilters`)
      .then(response => response.json())
      .then(data => {
        
        let filteredSeasons = data.allSeasons.filter(s => s.slice(0, 4) >= 1992 && s.slice(7, 11) <= 2018);
        
        this.setState({
          allTiers: data.allTiers,
          allSeasons: filteredSeasons,
          selectedTier: this.getDefaultTier(data.allTiers),
          selectedSeason: this.getDefaultSeason(filteredSeasons),
          isLoading: false
        });
      });
  };
  
  getDefaultTier(tiers)
  {
    return tiers.filter(t => t.level === 1)[0];
  }
  
  getDefaultSeason(seasons)
  {
    return seasons.sort().slice(-1)[0];
  }

  getDivisionName(tier, season) {
    const seasonStartYear = season.substring(0, 4);
    const seasonEndYear = season.substring(7);

    const divisions = tier.divisions.filter(d => seasonStartYear >= d.activeFrom && seasonEndYear <= d.activeTo);
    return divisions[0].name;
  }

  render() {
    const { allTiers, allSeasons, selectedTier, selectedSeason, isLoading } = this.state;

    if (isLoading) {
      return <p><em>Loading...</em></p>;
    }

    return (
      <React.Fragment>
        <h1>{this.getDivisionName(selectedTier, selectedSeason)}</h1>
        <h2>{selectedSeason}</h2>
        
        <Filter 
          allTiers={allTiers}
          allSeasons={allSeasons}
          selectedTier={selectedTier} 
          selectedSeason={selectedSeason}
          updateFilter={(selectedTier, selectedSeason) => this.updateFilter(selectedTier, selectedSeason)}
        />

        <div className='table-container'>
          <LeagueTable tier={selectedTier} season={selectedSeason} />
          {selectedTier.level !== 1 && <PlayOffMatches tier={selectedTier} season={selectedSeason} />}
        </div>

        <ResultMatrix tier={selectedTier} season={selectedSeason} />
      </React.Fragment>
    );
  };
}

export default Page;
