import React, { FunctionComponent, useEffect } from "react";
import { Row, useLeagueTable } from "../ClubPage/useLeagueTable";
import { Table } from "semantic-ui-react";

const LeagueTableRowCell: FunctionComponent<{
  bold: boolean;
  color: string | null;
}> = ({ children, bold, color }) => {
  return (
    <Table.Cell
      style={bold ? { fontWeight: "bold", backgroundColor: color } : { backgroundColor: color }}
    >
      {children}
    </Table.Cell>
  );
};

const LeagueTableRow: FunctionComponent<{ row: Row; club: string }> = ({ row, club }) => {
  const bold = row.team === club;
  const color =
    row.status === "Champions" ? "#75B266" : row.status === "Relegated" ? "#B26694" : null;

  return (
    <Table.Row key={row.position}>
      <LeagueTableRowCell bold={bold} color={color}>
        {row.position}
      </LeagueTableRowCell>
      <LeagueTableRowCell bold={bold} color={color}>
        {row.team}{" "}
      </LeagueTableRowCell>
      <LeagueTableRowCell bold={bold} color={color}>
        {row.played}
      </LeagueTableRowCell>
      <LeagueTableRowCell bold={bold} color={color}>
        {row.won}
      </LeagueTableRowCell>
      <LeagueTableRowCell bold={bold} color={color}>
        {row.drawn}
      </LeagueTableRowCell>
      <LeagueTableRowCell bold={bold} color={color}>
        {row.lost}
      </LeagueTableRowCell>
      <LeagueTableRowCell bold={bold} color={color}>
        {row.goalsFor}
      </LeagueTableRowCell>
      <LeagueTableRowCell bold={bold} color={color}>
        {row.goalsAgainst}
      </LeagueTableRowCell>
      <LeagueTableRowCell bold={bold} color={color}>
        {row.goalDifference}
      </LeagueTableRowCell>
      <LeagueTableRowCell bold={bold} color={color}>
        {row.points}
      </LeagueTableRowCell>
      <LeagueTableRowCell bold={bold} color={color}>
        {row.status}
      </LeagueTableRowCell>
    </Table.Row>
  );
};

const LeagueTable: FunctionComponent<{
  club: string;
  seasonStartYear: number | undefined;
  style: React.CSSProperties;
}> = ({ club, seasonStartYear, style }) => {
  const { leagueTable, getLeagueTable } = useLeagueTable();

  useEffect(() => {
    if (seasonStartYear !== undefined) {
      getLeagueTable(club, seasonStartYear);
    }
  }, [club, seasonStartYear]);

  return (
    <div style={{ ...style }}>
      {leagueTable === undefined || leagueTable.table === null ? (
        <div
          style={{
            height: "100px",
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
          }}
        >
          <p style={{ fontWeight: "bold" }}>
            The selected club was not in the Football League or Premier League in the season
            specified.
          </p>
        </div>
      ) : (
        <Table basic={"very"} striped collapsing>
          <Table.Header>
            <Table.Row>
              <Table.HeaderCell></Table.HeaderCell>
              <Table.HeaderCell></Table.HeaderCell>
              <Table.HeaderCell>P</Table.HeaderCell>
              <Table.HeaderCell>W</Table.HeaderCell>
              <Table.HeaderCell>D</Table.HeaderCell>
              <Table.HeaderCell>L</Table.HeaderCell>
              <Table.HeaderCell>GF</Table.HeaderCell>
              <Table.HeaderCell>GA</Table.HeaderCell>
              <Table.HeaderCell>Diff</Table.HeaderCell>
              <Table.HeaderCell>Points</Table.HeaderCell>
              <Table.HeaderCell></Table.HeaderCell>
            </Table.Row>
          </Table.Header>
          <Table.Body>
            {leagueTable && leagueTable.table.map((r) => <LeagueTableRow row={r} club={club} />)}
          </Table.Body>
        </Table>
      )}
    </div>
  );
};

export { LeagueTable };
