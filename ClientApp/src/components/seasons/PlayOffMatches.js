import React, { Component } from 'react';
import { Table } from 'react-bootstrap';
import './PlayOffMatches.css';

class PlayOffMatches extends Component {
  render() {
    const {playOffs} = this.props;

    return (
      <div className='playoff-container'>
        <div className='playoff-semifinal-container'>
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
                <td>{playOffs.semiFinals[0].firstLeg.homeTeam}</td>
                <td>{playOffs.semiFinals[0].firstLeg.homeGoals}</td>
                <td>{playOffs.semiFinals[0].secondLeg.awayGoals}</td>
                <td>{playOffs.semiFinals[0].secondLeg.awayGoalsET}</td>
                <td>{playOffs.semiFinals[0].secondLeg.awayGoalsET === null ? playOffs.semiFinals[0].secondLeg.awayGoalsET : `${playOffs.semiFinals[0].secondLeg.awayPenaltiesScored} (${playOffs.semiFinals[0].secondLeg.awayPenaltiesTaken})`}</td>
              </tr>
              <tr>
                <td>{playOffs.semiFinals[0].firstLeg.awayTeam}</td>
                <td>{playOffs.semiFinals[0].firstLeg.awayGoals}</td>
                <td>{playOffs.semiFinals[0].secondLeg.homeGoals}</td>
                <td>{playOffs.semiFinals[0].secondLeg.homeGoalsET}</td>
                <td>{playOffs.semiFinals[0].secondLeg.homeGoalsET === null ? playOffs.semiFinals[0].secondLeg.homeGoalsET : `${playOffs.semiFinals[0].secondLeg.homePenaltiesScored} (${playOffs.semiFinals[0].secondLeg.homePenaltiesTaken})`}</td>
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
                <td>{playOffs.semiFinals[1].firstLeg.homeTeam}</td>
                <td>{playOffs.semiFinals[1].firstLeg.homeGoals}</td>
                <td>{playOffs.semiFinals[1].secondLeg.awayGoals}</td>
                <td>{playOffs.semiFinals[1].secondLeg.awayGoalsET}</td>
                <td>{playOffs.semiFinals[1].secondLeg.awayGoalsET === null ? playOffs.semiFinals[1].secondLeg.awayGoalsET : `${playOffs.semiFinals[1].secondLeg.awayPenaltiesScored} (${playOffs.semiFinals[1].secondLeg.awayPenaltiesTaken})`}</td>
              </tr>
              <tr>
                <td>{playOffs.semiFinals[1].firstLeg.awayTeam}</td>
                <td>{playOffs.semiFinals[1].firstLeg.awayGoals}</td>
                <td>{playOffs.semiFinals[1].secondLeg.homeGoals}</td>
                <td>{playOffs.semiFinals[1].secondLeg.homeGoalsET}</td>
                <td>{playOffs.semiFinals[1].secondLeg.homeGoalsET === null ? playOffs.semiFinals[1].secondLeg.homeGoalsET : `${playOffs.semiFinals[1].secondLeg.homePenaltiesScored} (${playOffs.semiFinals[1].secondLeg.homePenaltiesTaken})`}</td>
              </tr>
            </tbody>
          </Table>
        </div>
        <div className='playoff-final-container'>
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
                <td>{playOffs.final.homeTeam}</td>
                <td>{playOffs.final.homeGoals}</td>
                <td>{playOffs.final.awayGoalsET}</td>
                <td>{playOffs.final.awayGoalsET === null ? playOffs.final.awayGoalsET : `${playOffs.final.awayPenaltiesScored} (${playOffs.final.awayPenaltiesTaken})`}</td>
              </tr>
              <tr>
                <td>{playOffs.final.awayTeam}</td>
                <td>{playOffs.final.awayGoals}</td>
                <td>{playOffs.final.homeGoalsET}</td>
                <td>{playOffs.final.homeGoalsET === null ? playOffs.final.homeGoalsET : `${playOffs.final.homePenaltiesScored} (${playOffs.final.homePenaltiesTaken})`}</td>
              </tr>
            </tbody>
          </Table>
        </div>
      </div>
    );
  }
}

export default PlayOffMatches;
