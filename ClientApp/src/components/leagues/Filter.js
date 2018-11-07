import React, { Component } from 'react';
import { ButtonToolbar, DropdownButton, MenuItem } from 'react-bootstrap';

class Filter extends Component {
  render() {
    const { competition, season } = this.props;

    return (
      <ButtonToolbar>
        <DropdownButton title="Division" id="DivisionSelect">
          <MenuItem eventKey="1" className={competition === "Premier League" ? "active" : ""}>Premier League</MenuItem>
          <MenuItem eventKey="2" className={competition === "Championship" || competition === "Division One" ? "active" : ""}>Championship (Division 1)</MenuItem>
          <MenuItem eventKey="3" className={competition === "League One" || competition === "Division Two" ? "active" : ""}>League 1 (Division 2)</MenuItem>
          <MenuItem eventKey="4" className={competition === "League Two" || competition === "Division Three" ? "active" : ""}>League 2 (Division 3)</MenuItem>
        </DropdownButton>
        <DropdownButton title="Season" id="SeasonSelect">
          <MenuItem eventKey="1">2010 - 2011</MenuItem>
          <MenuItem eventKey="2">2011 - 2012</MenuItem>
          <MenuItem eventKey="3">2012 - 2013</MenuItem>
          <MenuItem eventKey="4">2013 - 2014</MenuItem>
        </DropdownButton>
      </ButtonToolbar>
    );
  }
}

export default Filter;
