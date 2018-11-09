import React, { Component } from 'react';
import Table from './Table';
import Filter from './Filter';

class Page extends Component {
  constructor(props) {
    super(props);
    this.state = { 
      allSeasons: [],
      allDivisions: [],
      selectedDivision: null,
      selectedSeason: null,
      loading: true };
  }

  componentDidMount() {
    fetch(`api/FootballHistory/GetLeagueFilterOptions`)
      .then(response => response.json())
      .then(data => {
        this.setState({ 
          allSeasons: data.allSeasons.sort().reverse(), 
          allDivisions: data.allDivisions, 
          loading: false });
      });
  }

  selectDivision(division) {
    this.setState({selectedDivision: division.name});
  }

  selectSeason(season) {
    this.setState({selectedSeason: season});
  }

  render() {
    let body;
    let {loading, allSeasons, allDivisions, selectedDivision, selectedSeason} = this.state;

    if (loading) {
      body = <p><em>Loading...</em></p>
    } else {
      var season = selectedSeason === null ? allSeasons[0] : selectedSeason;
      var division = selectedDivision === null ? "Premier League" : selectedDivision;

      body = <React.Fragment>
        <Filter 
          allSeasons={allSeasons} 
          allDivisions={allDivisions} 
          selectedDivision={division} 
          selectedSeason={season}
          selectSeason={(s) => this.selectSeason(s)}
          selectDivision={(d) => this.selectDivision(d)}
        />
        <Table season={season} division={division} />
      </React.Fragment>
    }

    return (
      <div>
        {body}
      </div>
    );
  }
}

export default Page;
