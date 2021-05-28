import React, { FunctionComponent } from "react";
import { Table } from "semantic-ui-react";
import { Match } from "../../../shared/useFetchLeagueMatches";

const PlayOffFinalReplayed: FunctionComponent<{
  final: Match[];
  style: React.CSSProperties;
}> = ({ final, style }) => {
  final.sort((a, b) => a.matchDate.valueOf() - b.matchDate.valueOf());
  const firstLeg = final[0];
  const secondLeg = final[1];
  const thirdLeg = final[2];

  const thirdLegHomeTeamIsFirstLegHomeTeam = thirdLeg.homeTeam.name === firstLeg.homeTeam.name;

  return (
    <Table striped size="small" compact style={{ ...style, margin: 0 }}>
      <Table.Header>
        <Table.Row>
          <Table.HeaderCell width={5}>Team</Table.HeaderCell>
          <Table.HeaderCell>1st</Table.HeaderCell>
          <Table.HeaderCell>2nd</Table.HeaderCell>
          <Table.HeaderCell>3rd</Table.HeaderCell>
          <Table.HeaderCell>A.E.T</Table.HeaderCell>
          <Table.HeaderCell>Pens</Table.HeaderCell>
        </Table.Row>
      </Table.Header>
      <Table.Body>
        <Table.Row>
          <Table.Cell>{firstLeg.homeTeam.name}</Table.Cell>
          <Table.Cell>{firstLeg.homeTeam.goals}</Table.Cell>
          <Table.Cell>{secondLeg.awayTeam.goals}</Table.Cell>
          <Table.Cell>
            {thirdLegHomeTeamIsFirstLegHomeTeam ? thirdLeg.homeTeam.goals : thirdLeg.awayTeam.goals}
          </Table.Cell>
          <Table.Cell>
            {thirdLegHomeTeamIsFirstLegHomeTeam
              ? thirdLeg.rules.extraTime === true && thirdLeg.homeTeam.goalsExtraTime
              : thirdLeg.rules.extraTime === true && thirdLeg.awayTeam.goalsExtraTime}
          </Table.Cell>
          <Table.Cell>
            {thirdLegHomeTeamIsFirstLegHomeTeam
              ? thirdLeg.rules.penalties === true &&
                `${thirdLeg.homeTeam.penaltiesScored} (${thirdLeg.homeTeam.penaltiesTaken})`
              : thirdLeg.rules.penalties === true &&
                `${thirdLeg.awayTeam.penaltiesScored} (${thirdLeg.awayTeam.penaltiesTaken})`}
          </Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell>{firstLeg.awayTeam.name}</Table.Cell>
          <Table.Cell>{firstLeg.awayTeam.goals}</Table.Cell>
          <Table.Cell>{secondLeg.homeTeam.goals}</Table.Cell>
          <Table.Cell>
            {thirdLegHomeTeamIsFirstLegHomeTeam ? thirdLeg.awayTeam.goals : thirdLeg.homeTeam.goals}
          </Table.Cell>
          <Table.Cell>
            {thirdLegHomeTeamIsFirstLegHomeTeam
              ? thirdLeg.rules.extraTime === true && thirdLeg.awayTeam.goalsExtraTime
              : thirdLeg.rules.extraTime === true && thirdLeg.homeTeam.goalsExtraTime}
          </Table.Cell>
          <Table.Cell>
            {thirdLegHomeTeamIsFirstLegHomeTeam
              ? thirdLeg.rules.penalties === true &&
                `${thirdLeg.awayTeam.penaltiesScored} (${thirdLeg.awayTeam.penaltiesTaken})`
              : thirdLeg.rules.penalties === true &&
                `${thirdLeg.homeTeam.penaltiesScored} (${thirdLeg.homeTeam.penaltiesTaken})`}
          </Table.Cell>
        </Table.Row>
      </Table.Body>
    </Table>
  );
};

export { PlayOffFinalReplayed };
