import React, {Component} from 'react';
import {ButtonToolbar, Dropdown} from 'react-bootstrap';
import DropdownButton from "../shared/DropdownButton";

class Filter extends Component {
  updateTier(tier) {
    const { updateFilter, selectedSeason } = this.props;
    updateFilter(this.getDivisionTier(tier), selectedSeason);
  };

  updateSeason(season) {
    const { updateFilter, selectedDivisionTier } = this.props;
    updateFilter(selectedDivisionTier, season);
  };

  getDivisionInfo(tier) {
    let divisionNames = tier.divisions.map(d => `${d.name} (${d.activeFrom} - ${d.activeTo})`);
    return divisionNames.join(', ');
  };
  
  getDivisionTier(expandedTier) {
    const tier = expandedTier.split(":")[0].split(" ")[1];
    return this.props.allDivisionTiers.filter(dt => dt.tier === parseInt(tier))[0];
  }

  render() {
    const { allDivisionTiers, allSeasons, selectedDivisionTier, selectedSeason } = this.props;

    return (
      <ButtonToolbar>
        <DropdownButton 
            buttonLabel="Division" 
            dropdownItems={allDivisionTiers.map(dt => `Tier ${dt.tier}: ${this.getDivisionInfo(dt)}`)} 
            currentSelection={this.getDivisionInfo(selectedDivisionTier)} 
            updateSelected={(t) => this.updateTier(t)} />
        <DropdownButton 
            buttonLabel="Season" 
            dropdownItems={allSeasons.sort().reverse()} 
            currentSelection={selectedSeason} 
            updateSelected={(s) => this.updateSeason(s)} />
      </ButtonToolbar>
    );
  };
}

export default Filter;
