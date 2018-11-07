import React, { Component } from 'react';
import Table from './Table';
import Filter from './Filter';

class Page extends Component {
  constructor(props) {
    super(props);
    this.state = { leagueTable: {}, loading: true };
  }

  componentDidMount() {
    // fetch all seasons, divisions 
    // pass latest season and premier league into table to fetch league table data
    // pass all into filter for creating options

    // move this fetch to the table
    fetch('api/FootballHistory/GetLeague?competition=Premier League&seasonStartYear=2010')
      .then(response => response.json())
      .then(data => {
        this.setState({ leagueTable: data, loading: false });
      });
  }

  render() {
    let {leagueTable, loading} = this.state;
    let body;

    if (loading) {
      body = <p><em>Loading...</em></p>
    } else {
      body = <React.Fragment>
        <Filter competition={leagueTable.competition} season={leagueTable.season} />
        <Table leagueTable={leagueTable} />
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
