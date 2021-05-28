import React, { FunctionComponent } from "react";
import { Table } from "semantic-ui-react";
import { Match } from "../../../shared/useFetchLeagueMatches";

const PlayOffFinalTwoLegged: FunctionComponent<{
  final: Match[];
  style: React.CSSProperties;
}> = ({ final, style }) => {
  final.sort((a, b) => a.matchDate.valueOf() - b.matchDate.valueOf());
  const firstLeg = final[0];
  const secondLeg = final[1];

  return (
    <Table striped size="small" compact style={{ ...style, margin: 0 }}>
      <Table.Header>
        <Table.Row>
          <Table.HeaderCell width={5}>Team</Table.HeaderCell>
          <Table.HeaderCell>1st</Table.HeaderCell>
          <Table.HeaderCell>2nd</Table.HeaderCell>
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
            {secondLeg.rules.extraTime === true && secondLeg.awayTeam.goalsExtraTime}
          </Table.Cell>
          <Table.Cell>
            {secondLeg.rules.penalties === true &&
              `${secondLeg.awayTeam.penaltiesScored} (${secondLeg.awayTeam.penaltiesTaken})`}
          </Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell>{firstLeg.awayTeam.name}</Table.Cell>
          <Table.Cell>{firstLeg.awayTeam.goals}</Table.Cell>
          <Table.Cell>{secondLeg.homeTeam.goals}</Table.Cell>
          <Table.Cell>
            {secondLeg.rules.extraTime === true && secondLeg.homeTeam.goalsExtraTime}
          </Table.Cell>
          <Table.Cell>
            {secondLeg.rules.penalties === true &&
              `${secondLeg.homeTeam.penaltiesScored} (${secondLeg.homeTeam.penaltiesTaken})`}
          </Table.Cell>
        </Table.Row>
      </Table.Body>
    </Table>
  );
};
export { PlayOffFinalTwoLegged };
