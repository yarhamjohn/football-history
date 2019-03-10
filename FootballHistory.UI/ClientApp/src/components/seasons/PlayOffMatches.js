import React, { Component } from 'react';
import { Table } from 'react-bootstrap';
import baseUrl from "../../api/LeagueSeasonApi";
import './PlayOffMatches.css';

class PlayOffMatches extends Component {
  constructor(props) {
    super(props);
    this.state = { 
      playOffs: [], 
      isLoading: true 
    };
  };

  fetchPlayOffMatches() {
    const { tier, seasonStartYear } = this.props;

      fetch(`${baseUrl}/api/LeagueSeason/GetPlayOffMatches?tier=${tier.level}&seasonStartYear=${seasonStartYear}`)
      .then(response => response.json())
      .then(data => {
        this.setState({ 
          playOffs: data,
          isLoading: false 
        });
      });
  };

  componentDidMount() {
    this.fetchPlayOffMatches();
  };

  componentDidUpdate(prevProps) {
    if (prevProps.tier !== this.props.tier || prevProps.seasonStartYear !== this.props.seasonStartYear) {
      this.setState({ isLoading: true }, this.fetchPlayOffMatches());
    }
  };

  render() {
    const { playOffs, isLoading } = this.state;

    if (isLoading) {
      return <p><em>Loading...</em></p>
    }

    return (
      <div className='playoff-container'>
        <div className='playoff-semifinal-container'>
        {
          playOffs.semiFinals.map(sf => <PlayOffSemiFinal key={sf.firstLeg.homeTeam + sf.firstLeg.awayTeam} semiFinal={sf} />)
        }
        </div>
        <div className='playoff-final-container'>
          <PlayOffFinal final={playOffs.final} />
        </div>
      </div>
    );
  }
}

function PlayOffSemiFinal(props) {
  const { semiFinal } = props;

  return (
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
          <td>{semiFinal.firstLeg.homeTeam}</td>
          <td>{semiFinal.firstLeg.homeGoals}</td>
          <td>{semiFinal.secondLeg.awayGoals}</td>
          <td>{semiFinal.secondLeg.awayGoalsET}</td>
          <td>
            {semiFinal.secondLeg.penaltyShootout === true && `${semiFinal.secondLeg.awayPenaltiesScored} (${semiFinal.secondLeg.awayPenaltiesTaken})`}
          </td>
        </tr>
        <tr>
          <td>{semiFinal.firstLeg.awayTeam}</td>
          <td>{semiFinal.firstLeg.awayGoals}</td>
          <td>{semiFinal.secondLeg.homeGoals}</td>
          <td>{semiFinal.secondLeg.homeGoalsET}</td>
          <td>
            {semiFinal.secondLeg.penaltyShootout === true && `${semiFinal.secondLeg.homePenaltiesScored} (${semiFinal.secondLeg.homePenaltiesTaken})`}
          </td>
        </tr>
      </tbody>
    </Table>
  )
}

function PlayOffFinal(props) {
  const { final } = props;

  return (
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
          <td>{final.homeTeam}</td>
          <td>{final.homeGoals}</td>
          <td>{final.homeGoalsET}</td>
          <td>{final.penaltyShootout === true && `${final.awayPenaltiesScored} (${final.awayPenaltiesTaken})`}</td>
        </tr>
        <tr>
          <td>{final.awayTeam}</td>
          <td>{final.awayGoals}</td>
          <td>{final.awayGoalsET}</td>
          <td>{final.penaltyShootout === true && `${final.homePenaltiesScored} (${final.homePenaltiesTaken})`}</td>
        </tr>
      </tbody>
    </Table>
  )
}

export default PlayOffMatches;
