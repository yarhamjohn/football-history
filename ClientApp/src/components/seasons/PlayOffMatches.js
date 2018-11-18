import React, { Component } from 'react';
import { Table } from 'react-bootstrap';

class PlayOffMatches extends Component {
  constructor(props) {
    super(props);
    this.state = { playOffMatches: {}, loading: true };
  }

  fetchPlayOffMatches() {
    const { season, tier } = this.props;

    fetch(`api/FootballHistory/GetPlayOffMatches?tier=${tier}&season=${season}`)
      .then(response => response.json())
      .then(data => {
        this.setState({ playOffMatches: data, loading: false });
      });
  }

  componentDidMount() {
    this.fetchPlayOffMatches();
  }

  componentDidUpdate(prevProps) {
    if (prevProps.season !== this.props.season || prevProps.tier !== this.props.tier) {
      this.fetchPlayOffMatches();
    }
  }

  render() {
    const {playOffMatches, loading} = this.state;
    let body;

    if (loading) {
      body = <p><em>Loading...</em></p>
    } else {
      body = <React.Fragment>
        <div style={{marginRight: 100, display: 'flex', justifyContent: 'space-between', flexDirection: 'column'}}>
          <Table striped condensed>
            <thead>
              <tr>
                <th>Team</th>
                <th>1st</th>
                <th>2nd</th>
                <th>A.E.T</th>
                <th>Pens</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>{playOffMatches.semiFinals[0].firstLeg.homeTeam}</td>
                <td>{playOffMatches.semiFinals[0].firstLeg.homeGoals}</td>
                <td>{playOffMatches.semiFinals[0].secondLeg.awayGoals}</td>
                <td>{playOffMatches.semiFinals[0].secondLeg.awayGoalsET}</td>
                <td>{playOffMatches.semiFinals[0].secondLeg.awayGoalsET === null ? playOffMatches.semiFinals[0].secondLeg.awayGoalsET : `${playOffMatches.semiFinals[0].secondLeg.awayPenaltiesScored} (${playOffMatches.semiFinals[0].secondLeg.awayPenaltiesTaken})`}</td>
              </tr>
              <tr>
                <td>{playOffMatches.semiFinals[0].firstLeg.awayTeam}</td>
                <td>{playOffMatches.semiFinals[0].firstLeg.awayGoals}</td>
                <td>{playOffMatches.semiFinals[0].secondLeg.homeGoals}</td>
                <td>{playOffMatches.semiFinals[0].secondLeg.homeGoalsET}</td>
                <td>{playOffMatches.semiFinals[0].secondLeg.homeGoalsET === null ? playOffMatches.semiFinals[0].secondLeg.homeGoalsET : `${playOffMatches.semiFinals[0].secondLeg.homePenaltiesScored} (${playOffMatches.semiFinals[0].secondLeg.homePenaltiesTaken})`}</td>
              </tr>
            </tbody>
          </Table>
          <Table striped condensed>
            <thead>
              <tr>
                <th>Team</th>
                <th>1st</th>
                <th>2nd</th>
                <th>A.E.T</th>
                <th>Pens</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>{playOffMatches.semiFinals[1].firstLeg.homeTeam}</td>
                <td>{playOffMatches.semiFinals[1].firstLeg.homeGoals}</td>
                <td>{playOffMatches.semiFinals[1].secondLeg.awayGoals}</td>
                <td>{playOffMatches.semiFinals[1].secondLeg.awayGoalsET}</td>
                <td>{playOffMatches.semiFinals[1].secondLeg.awayGoalsET === null ? playOffMatches.semiFinals[1].secondLeg.awayGoalsET : `${playOffMatches.semiFinals[1].secondLeg.awayPenaltiesScored} (${playOffMatches.semiFinals[1].secondLeg.awayPenaltiesTaken})`}</td>
              </tr>
              <tr>
                <td>{playOffMatches.semiFinals[1].firstLeg.awayTeam}</td>
                <td>{playOffMatches.semiFinals[1].firstLeg.awayGoals}</td>
                <td>{playOffMatches.semiFinals[1].secondLeg.homeGoals}</td>
                <td>{playOffMatches.semiFinals[1].secondLeg.homeGoalsET}</td>
                <td>{playOffMatches.semiFinals[1].secondLeg.homeGoalsET === null ? playOffMatches.semiFinals[1].secondLeg.homeGoalsET : `${playOffMatches.semiFinals[1].secondLeg.homePenaltiesScored} (${playOffMatches.semiFinals[1].secondLeg.homePenaltiesTaken})`}</td>
              </tr>
            </tbody>
          </Table>
        </div>
        <div style={{alignItems: 'center', display: 'flex'}}>
          <Table striped condensed>
            <thead>
              <tr>
                <th>Team</th>
                <th>Score</th>
                <th>A.E.T</th>
                <th>Pens</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>{playOffMatches.final.homeTeam}</td>
                <td>{playOffMatches.final.homeGoals}</td>
                <td>{playOffMatches.final.awayGoalsET}</td>
                <td>{playOffMatches.final.awayGoalsET === null ? playOffMatches.final.awayGoalsET : `${playOffMatches.final.awayPenaltiesScored} (${playOffMatches.final.awayPenaltiesTaken})`}</td>
              </tr>
              <tr>
                <td>{playOffMatches.final.awayTeam}</td>
                <td>{playOffMatches.final.awayGoals}</td>
                <td>{playOffMatches.final.homeGoalsET}</td>
                <td>{playOffMatches.final.homeGoalsET === null ? playOffMatches.final.homeGoalsET : `${playOffMatches.final.homePenaltiesScored} (${playOffMatches.final.homePenaltiesTaken})`}</td>
              </tr>
            </tbody>
          </Table>
        </div>
      </React.Fragment>
    }

    return (
      <div style={{display: 'flex', height: 250}}>
        {body}
      </div>
    );
  }
}

export default PlayOffMatches;
