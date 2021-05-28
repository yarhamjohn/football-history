import React, { FunctionComponent } from "react";
import { Table } from "semantic-ui-react";
import { League } from "../../../shared/useFetchLeague";
import { LeagueTableRow } from "./Row";
import { Team } from "../../../shared/useFetchClubs";

const LeagueTable: FunctionComponent<{
  league: League;
  selectedClub: Team | undefined;
  seasonStartYear: number;
}> = ({ league, selectedClub, seasonStartYear }) => {
  const rows = league.table.map((r) => (
    <LeagueTableRow
      key={r.position}
      row={r}
      selectedClub={selectedClub}
      seasonStartYear={seasonStartYear}
      numRows={league.table.length}
      rules={league.competition.rules}
      competitionId={league.competition.id}
    />
  ));

  return (
    <Table basic compact>
      <Table.Header>
        <Table.Row>
          <Table.HeaderCell></Table.HeaderCell>
          <Table.HeaderCell></Table.HeaderCell>
          <Table.HeaderCell></Table.HeaderCell>
          <Table.HeaderCell>P</Table.HeaderCell>
          <Table.HeaderCell>W</Table.HeaderCell>
          <Table.HeaderCell>D</Table.HeaderCell>
          <Table.HeaderCell>L</Table.HeaderCell>
          <Table.HeaderCell>GF</Table.HeaderCell>
          <Table.HeaderCell>GA</Table.HeaderCell>
          <Table.HeaderCell>Diff</Table.HeaderCell>
          <Table.HeaderCell>GAv</Table.HeaderCell>
          <Table.HeaderCell>PPG</Table.HeaderCell>
          <Table.HeaderCell>Points</Table.HeaderCell>
          <Table.HeaderCell></Table.HeaderCell>
        </Table.Row>
      </Table.Header>
      <Table.Body>{rows}</Table.Body>
    </Table>
  );
};

export { LeagueTable };
