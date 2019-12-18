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
      allDivisionTiers: [],
      allSeasons: [],
      selectedDivisionTier: null,
      selectedSeason: null,
      isLoading: true 
    };
  };

  updateFilter(selectedTier, selectedSeason) {
    this.setState({ 
      selectedDivisionTier: selectedTier, 
      selectedSeason: selectedSeason
    });
  };

  componentDidMount() {
    fetch(`${baseUrl}/api/LeagueSeason/GetLeagueSeasonFilters`)
      .then(response => response.json())
      .then(data => {
        
        let filteredSeasons = data.allSeasons.filter(s => s.slice(0, 4) >= 1992 && s.slice(7, 11) <= 2019);
        
        this.setState({
          allDivisionTiers: data.allTiers,
          allSeasons: filteredSeasons,
          selectedDivisionTier: this.getDefaultDivisionTier(data.allTiers),
          selectedSeason: this.getDefaultSeason(filteredSeasons),
          isLoading: false
        });
      });
  };
  
  getDefaultDivisionTier(tiers)
  {
    return tiers.filter(t => t.tier === 1)[0];
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
    const { allDivisionTiers, allSeasons, selectedDivisionTier, selectedSeason, isLoading } = this.state;

    if (isLoading) {
      return <p><em>Loading...</em></p>;
    }

    return (
      <React.Fragment>
        <h1 className='header'>{this.getDivisionName(selectedDivisionTier, selectedSeason)} ({selectedSeason})</h1>
        
        <div className='filter-container'>
          <Filter 
            allDivisionTiers={allDivisionTiers}
            allSeasons={allSeasons}
            selectedDivisionTier={selectedDivisionTier} 
            selectedSeason={selectedSeason}
            updateFilter={(selectedDivisionTier, selectedSeason) => this.updateFilter(selectedDivisionTier, selectedSeason)}
          />
        </div>

        <div className='table-container'>
          <LeagueTable tier={selectedDivisionTier.tier} seasonStartYear={selectedSeason.substring(0, 4)} />
          {selectedDivisionTier.tier !== 1 && <PlayOffMatches tier={selectedDivisionTier.tier} seasonStartYear={selectedSeason.substring(0, 4)} />}
        </div>

        <ResultMatrix tier={selectedDivisionTier.tier} seasonStartYear={selectedSeason.substring(0, 4)} />
      </React.Fragment>
    );
  };
}

export default Page;
