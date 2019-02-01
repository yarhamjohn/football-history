import React, { Component } from 'react';
import { ButtonToolbar, DropdownButton, MenuItem } from 'react-bootstrap';
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
      <ButtonToolbar className='filter-buttons'>
        <DropdownButton title="Division" id="DivisionSelect">
        {
          allTiers.map(t => 
            <MenuItem key={t.level} eventKey={t} 
              className={t.level === selectedTier ? "active" : ""}
              onSelect={(t) => this.updateTier(t)}
            >
              {`Tier ${t.level}: ${this.getDivisionInfo(t)}`}
            </MenuItem>)
        }
        </DropdownButton>
        <DropdownButton title="Season" id="SeasonSelect">
        {
          allSeasons.sort().reverse().map(s =>
            <MenuItem key={s} eventKey={s}
              className={s === selectedSeason ? "active" : ""}
              onSelect={(s) => this.updateSeason(s)}
            >
              {s}
            </MenuItem>)
        }
        </DropdownButton>
      </ButtonToolbar>
    );
  };
}

export default Filter;
