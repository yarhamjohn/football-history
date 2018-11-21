import React, { Component } from 'react';
import { ButtonToolbar, DropdownButton, MenuItem } from 'react-bootstrap';

class Filter extends Component {
  selectTier(tier) {
    const { selectLeagueTable, selectedSeason } = this.props;
    selectLeagueTable(tier, selectedSeason);
  };

  selectSeason(season) {
    const { selectLeagueTable, selectedTier } = this.props;
    selectLeagueTable(selectedTier, season);
  };

  getDivisionInfo(tier) {
    let divisionNames = tier.divisions.map(d => `${d.name} (${d.seasonStartYear} - ${d.seasonEndYear})`);
    return divisionNames.join(', ');
  }

  render() {
    const { allSeasons, allTiers, selectedTier, selectedSeason } = this.props;

    return (
        <ButtonToolbar style={{marginTop: 20}}>
          <DropdownButton title="League" id="LeagueSelect">
          {
            allTiers.map(t => 
              <MenuItem key={t.level}
                eventKey={t} 
                className={t.level === selectedTier ? "active" : ""}
                onSelect={(t) => this.selectTier(t)}
              >
                {`Tier ${t.level}: ${this.getDivisionInfo(t)}`}
              </MenuItem>)
          }
          </DropdownButton>
          <DropdownButton title="Season" id="SeasonSelect">
          {
            allSeasons.map(s =>
              <MenuItem key={s} eventKey={s}
                className={s === selectedSeason ? "active" : ""}
                onSelect={(s) => this.selectSeason(s)}
              >
                {s}
              </MenuItem>)
          }
          </DropdownButton>
        </ButtonToolbar>
    );
  }
}

export default Filter;
