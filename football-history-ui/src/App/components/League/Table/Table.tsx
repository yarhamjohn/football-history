import React, { FunctionComponent } from "react";
import { Table } from "semantic-ui-react";
import { League } from "../../../shared/useFetchLeague";
import { LeagueTableRow } from "./Row";
import { Team } from "../../../shared/teamsSlice";

const LeagueTable: FunctionComponent<{
  league: League;
  selectedTeam: Team | undefined;
  seasonStartYear: number;
}> = ({ league, selectedTeam, seasonStartYear }) => {
  const rows = league.table.map((r) => (
    <LeagueTableRow
      key={r.position}
      row={r}
      selectedTeam={selectedTeam}
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
