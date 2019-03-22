import React, { Component } from 'react';
import { ButtonToolbar, DropdownButton, DropdownItem } from 'react-bootstrap';
import './Filter.css';

class Filter extends Component {
  updateTier(tier) {
    const { updateFilter, selectedSeason } = this.props;
    updateFilter(tier, selectedSeason);
  };

  updateSeason(season) {
    const { updateFilter, selectedTier } = this.props;
    updateFilter(selectedTier, season);
  };

  getDivisionInfo(tier) {
    let divisionNames = tier.divisions.map(d => `${d.name} (${d.activeFrom} - ${d.activeTo})`);
    return divisionNames.join(', ');
  };

  render() {
    const { allTiers, allSeasons, selectedTier, selectedSeason } = this.props;

    return (
      <ButtonToolbar>
        <DropdownButton className='filter-button' variant='outline-dark' title="Division" id="DivisionSelect">
        {
          allTiers.map(t => 
            <DropdownItem key={t.tier} eventKey={t} 
              className={t.tier === selectedTier ? "active" : ""}
              onSelect={(t) => this.updateTier(t)}
            >
              {`Tier ${t.tier}: ${this.getDivisionInfo(t)}`}
            </DropdownItem>)
        }
        </DropdownButton>
        <DropdownButton className='filter-button' variant='outline-dark' title="Season" id="SeasonSelect">
        {
          allSeasons.sort().reverse().map(s =>
            <DropdownItem key={s} eventKey={s}
              className={s === selectedSeason ? "active" : ""}
              onSelect={(s) => this.updateSeason(s)}
            >
              {s}
            </DropdownItem>)
        }
        </DropdownButton>
      </ButtonToolbar>
    );
  };
}

export default Filter;
