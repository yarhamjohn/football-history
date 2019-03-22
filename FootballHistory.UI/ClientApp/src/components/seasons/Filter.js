import React, { Component } from 'react';
import { ButtonToolbar, DropdownButton, DropdownItem } from 'react-bootstrap';
import './Filter.css';

class Filter extends Component {
  updateTier(tier) {
    const { updateFilter, selectedSeason } = this.props;
    updateFilter(tier, selectedSeason);
  };

  updateSeason(season) {
    const { updateFilter, selectedDivisionTier } = this.props;
    updateFilter(selectedDivisionTier, season);
  };

  getDivisionInfo(tier) {
    let divisionNames = tier.divisions.map(d => `${d.name} (${d.activeFrom} - ${d.activeTo})`);
    return divisionNames.join(', ');
  };

  render() {
    const { allDivisionTiers, allSeasons, selectedDivisionTier, selectedSeason } = this.props;

    return (
      <ButtonToolbar>
        <DropdownButton className='filter-button' variant='outline-dark' title="Division" id="DivisionSelect">
        {
          allDivisionTiers.map(dt => 
            <DropdownItem key={dt.tier} 
              className={dt.tier === selectedDivisionTier.tier ? "active" : ""}
              onSelect={() => this.updateTier(dt)}
            >
              {`Tier ${dt.tier}: ${this.getDivisionInfo(dt)}`}
            </DropdownItem>)
        }
        </DropdownButton>
        <DropdownButton className='filter-button' variant='outline-dark' title="Season" id="SeasonSelect">
        {
          allSeasons.sort().reverse().map(s =>
            <DropdownItem key={s}
              className={s === selectedSeason ? "active" : ""}
              onSelect={() => this.updateSeason(s)}
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
