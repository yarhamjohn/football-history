import React, { FunctionComponent, useEffect } from "react";
import { PlayOffMatch, PlayOffMatchesState, usePlayOffMatches } from "../hooks/usePlayOffMatches";
import { Table } from "semantic-ui-react";

const PlayOffFinal: FunctionComponent<{ final: PlayOffMatch; style: React.CSSProperties }> = ({
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
          <Table.Cell>{final.homeTeam}</Table.Cell>
          <Table.Cell>{final.homeGoals}</Table.Cell>
          <Table.Cell>{final.homeGoalsExtraTime}</Table.Cell>
          <Table.Cell>
            {final.penaltyShootout === true &&
              `${final.awayPenaltiesScored} (${final.awayPenaltiesTaken})`}
          </Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell>{final.awayTeam}</Table.Cell>
          <Table.Cell>{final.awayGoals}</Table.Cell>
          <Table.Cell>{final.awayGoalsExtraTime}</Table.Cell>
          <Table.Cell>
            {final.penaltyShootout === true &&
              `${final.homePenaltiesScored} (${final.homePenaltiesTaken})`}
          </Table.Cell>
        </Table.Row>
      </Table.Body>
    </Table>
  );
};

const PlayOffSemiFinal: FunctionComponent<{
  semiFinal: PlayOffMatch[];
  style: React.CSSProperties;
}> = ({ semiFinal, style }) => {
  if (semiFinal.length !== 2) {
    throw new Error(
      `A play off semi final should consist of two matches. ${semiFinal.length} were provided.`
    );
  }

  semiFinal.sort((a, b) => b.date.valueOf() - a.date.valueOf());
  const firstLeg = semiFinal[0];
  const secondLeg = semiFinal[1];

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
          <Table.Cell>{firstLeg.homeTeam}</Table.Cell>
          <Table.Cell>{firstLeg.homeGoals}</Table.Cell>
          <Table.Cell>{secondLeg.awayGoals}</Table.Cell>
          <Table.Cell>{secondLeg.awayGoalsExtraTime}</Table.Cell>
          <Table.Cell>
            {secondLeg.penaltyShootout === true &&
              `${secondLeg.awayPenaltiesScored} (${secondLeg.awayPenaltiesTaken})`}
          </Table.Cell>
        </Table.Row>
        <Table.Row>
          <Table.Cell>{firstLeg.awayTeam}</Table.Cell>
          <Table.Cell>{firstLeg.awayGoals}</Table.Cell>
          <Table.Cell>{secondLeg.homeGoals}</Table.Cell>
          <Table.Cell>{secondLeg.homeGoalsExtraTime}</Table.Cell>
          <Table.Cell>
            {secondLeg.penaltyShootout === true &&
              `${secondLeg.homePenaltiesScored} (${secondLeg.homePenaltiesTaken})`}
          </Table.Cell>
        </Table.Row>
      </Table.Body>
    </Table>
  );
};

const PlayOffs: FunctionComponent<{
  matches: PlayOffMatch[];
}> = ({ matches }) => {
  if (matches.length === 0) {
    return null;
  }

  const semiFinals = matches.filter((m) => m.round === "Semi-Final");
  const teams = semiFinals.map((m) => m.homeTeam);
  const semiFinalOne = semiFinals.filter((m) => m.homeTeam === teams[0] || m.awayTeam === teams[0]);
  const semiFinalTwo = semiFinals.filter((m) => m.homeTeam !== teams[0] && m.awayTeam !== teams[0]);

  return (
    <div
      style={{
        display: "grid",
        gridTemplateRows: "auto auto 1rem auto auto",
        gridTemplateColumns: "auto auto 1rem auto auto",
        gridTemplateAreas:
          "'. one . . .' '. one . final .' '. . . final .' '. two . final .' '. two . . .'",
      }}
    >
      {<PlayOffSemiFinal semiFinal={semiFinalOne} style={{ gridArea: "one" }} />}
      {<PlayOffSemiFinal semiFinal={semiFinalTwo} style={{ gridArea: "two", marginTop: 0 }} />}
      {
        <PlayOffFinal
          final={matches.filter((m) => m.round === "Final")[0]}
          style={{ gridArea: "final" }}
        />
      }
    </div>
  );
};

export { PlayOffs };
