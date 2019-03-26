import React, {Component} from 'react';
import {ButtonToolbar, Dropdown} from 'react-bootstrap';
import DropdownButton from "../shared/DropdownButton";

class Filter extends Component {
  updateTier(tier) {
    const { updateFilter, selectedSeason } = this.props;
    let divisionTier = this.getDivisionTier(tier);
    updateFilter(divisionTier, selectedSeason);
  };

  updateSeason(season) {
    const { updateFilter, selectedDivisionTier } = this.props;
    updateFilter(selectedDivisionTier, season);
  };

  getDivisionTier(expandedTier) {
    const tier = expandedTier.split(":")[0].split(" ")[1];
    return this.props.allDivisionTiers.filter(dt => dt.tier === parseInt(tier))[0];
  }

  getDropdownString(dt) {
    return `Tier ${dt.tier}: ${this.getDivisionInfo(dt)}`;
  }

  getDivisionInfo(tier) {
    let divisionNames = tier.divisions.map(d => `${d.name} (${d.activeFrom} - ${d.activeTo})`);
    return divisionNames.join(', ');
  };

  render() {
    const { allDivisionTiers, allSeasons, selectedDivisionTier, selectedSeason } = this.props;
    let sortedSeasons = allSeasons.sort().reverse();
    let divisions = allDivisionTiers.map(dt => this.getDropdownString(dt));
    let selectedDivision = this.getDropdownString(selectedDivisionTier);

    return (
      <ButtonToolbar>
        <DropdownButton 
            buttonLabel="Division" 
            dropdownItems={divisions} 
            currentSelection={selectedDivision} 
            updateSelected={(t) => this.updateTier(t)} />
        <DropdownButton 
            buttonLabel="Season" 
            dropdownItems={sortedSeasons} 
            currentSelection={selectedSeason} 
            updateSelected={(s) => this.updateSeason(s)} />
      </ButtonToolbar>
    );
  };
}

export default Filter;
