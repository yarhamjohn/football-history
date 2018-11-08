import React, { Component } from 'react';
import { ButtonToolbar, DropdownButton, MenuItem } from 'react-bootstrap';

class Filter extends Component {
  render() {
    const { allSeasons, allDivisions, selectedDivision, selectedSeason } = this.props;

    return (
      <ButtonToolbar>
        <DropdownButton title="Division" id="DivisionSelect">
        {
          allDivisions.map(d => 
            <MenuItem key={d} eventKey={d} 
              className={d === selectedDivision ? "active" : ""}
            >
              {d}
            </MenuItem>)
        }
        </DropdownButton>
        <DropdownButton title="Season" id="SeasonSelect">
        {
          allSeasons.map(s =>
            <MenuItem key={s} eventKey={s}
              className={s === selectedSeason ? "active" : ""}
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
