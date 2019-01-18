import React, { Component } from 'react';
import Filter from './Filter';
import LeagueTable from './leagueTable/LeagueTable';
import PlayOffMatches from './PlayOffMatches';
import ResultMatrix from './ResultMatrix';
import './Page.css';

class Page extends Component {
  constructor(props) {
    super(props);
    this.state = { 
      tier: null,
      season: null,
      isLoading: true 
    };
  };

  updateFilter(selectedTier, selectedSeason) {
    this.setState({ 
      tier: selectedTier, 
      season: selectedSeason
    });
  };

  componentDidMount() {
    fetch(`api/LeagueSeason/GetDefaultFilter`)
      .then(response => response.json())
      .then(data => {
        this.setState({ 
          tier: data.tier,
          season: data.season,
          isLoading: false 
        });
      });
  };

  getDivisionName(tier, season) {
    const seasonStartYear = season.substring(0, 4);
    const seasonEndYear = season.substring(7);

    const divisions = tier.divisions.filter(d => seasonStartYear >= d.activeFrom && seasonEndYear <= d.activeTo);
    return divisions[0].name;
  }

  render() {
    const { tier, season, isLoading } = this.state;

    if (isLoading) {
      return <p><em>Loading...</em></p>;
    }

    return (
      <React.Fragment>
        <h1>{this.getDivisionName(tier, season)}</h1>
        <h2>{season}</h2>
        
        <Filter 
          selectedTier={tier} 
          selectedSeason={season}
          updateFilter={(tier, season) => this.updateFilter(tier, season)}
        />

        <div className='table-container'>
          <LeagueTable tier={tier} season={season} />
          {tier.level !== 1 && <PlayOffMatches tier={tier} season={season} />}
        </div>

        <ResultMatrix tier={tier} season={season} />
      </React.Fragment>
    );
  };
}

export default Page;
