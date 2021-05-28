import React, { FunctionComponent } from "react";
import { Table } from "semantic-ui-react";
import { Match } from "../../../shared/useFetchLeagueMatches";

const PlayOffFinalOneLegged: FunctionComponent<{ final: Match; style: React.CSSProperties }> = ({
  final,
  style,
}) => {
  return (
    <Table striped size="small" compact style={{ ...style, margin: 0 }}>
      <Table.Header>
        <Table.Row>
          <Table.HeaderCell width={5}>Team</Table.HeaderCell>
          <Table.HeaderCell>Score</Table.HeaderCell>
          <Table.HeaderCell>A.E.T</Table.HeaderCell>
          <Table.HeaderCell>Pens</Table.HeaderCell>
        </Table.Row>
      </Table.Header>
      <Table.Body>
        <Table.Row>
          <Table.Cell>{final.homeTeam.name}</Table.Cell>
          <Table.Cell>{final.homeTeam.goals}</Table.Cell>
          <Table.Cell>{final.rules.extraTime === true && final.homeTeam.goalsExtraTime}</Table.Cell>
          <Table.Cell>
            {final.rules.penalties === true &&
              `${final.awayTeam.penaltiesScored} (${final.awayTeam.penaltiesTaken})`}
          </Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell>{final.awayTeam.name}</Table.Cell>
          <Table.Cell>{final.awayTeam.goals}</Table.Cell>
          <Table.Cell>{final.rules.extraTime === true && final.awayTeam.goalsExtraTime}</Table.Cell>
          <Table.Cell>
            {final.rules.penalties === true &&
              `${final.homeTeam.penaltiesScored} (${final.homeTeam.penaltiesTaken})`}
          </Table.Cell>
        </Table.Row>
      </Table.Body>
    </Table>
  );
};

export { PlayOffFinalOneLegged };
