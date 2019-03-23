import React, { Component } from 'react';
import {ButtonToolbar, Dropdown} from 'react-bootstrap';
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
        <Dropdown className='filter-button'>
          <Dropdown.Toggle variant='outline-primary'>Division</Dropdown.Toggle>
          <Dropdown.Menu flip={false}>
            {
              allDivisionTiers.map(dt =>
                  <Dropdown.Item key={dt.tier}
                                className={dt.tier === selectedDivisionTier.tier ? "active" : ""}
                                onSelect={() => this.updateTier(dt)}
                  >
                    {`Tier ${dt.tier}: ${this.getDivisionInfo(dt)}`}
                  </Dropdown.Item>)
            }
          </Dropdown.Menu>
        </Dropdown>

        <Dropdown className='filter-button'>
          <Dropdown.Toggle variant='outline-primary'>Season</Dropdown.Toggle>
          <Dropdown.Menu flip={false}>
            {
              allSeasons.sort().reverse().map(s =>
                  <Dropdown.Item key={s} 
                                className={s === selectedSeason ? "active" : ""}
                                onSelect={() => this.updateSeason(s)}
                  >
                    {s}
                  </Dropdown.Item>)
            }
          </Dropdown.Menu>
        </Dropdown>
      </ButtonToolbar>
    );
  };
}

export default Filter;
