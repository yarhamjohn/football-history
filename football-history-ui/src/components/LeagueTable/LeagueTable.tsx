import React, { FunctionComponent, useEffect } from "react";
import { League } from "./useLeagueTable";
import { Table } from "semantic-ui-react";
import { LeagueTableRow } from "./LeagueTableRow";
import { PointDeductionSummary } from "./PointDeductionSummary";

const LeagueTable: FunctionComponent<{
  club?: string;
  table: League;
  seasonStartYear: number | undefined;
}> = ({ club, table, seasonStartYear }) => {
  function getNumRows() {
    if (table && table.table) {
      return table.table.length;
    }

    return 0;
  }

  return (
    <div>
      {table === undefined || table.table === null ? (
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
        <>
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
                <Table.HeaderCell>Points</Table.HeaderCell>
                <Table.HeaderCell></Table.HeaderCell>
              </Table.Row>
            </Table.Header>
            <Table.Body>
              {table &&
                seasonStartYear &&
                table.table.map((r) => (
                  <LeagueTableRow
                    key={r.position}
                    row={r}
                    club={club}
                    seasonStartYear={seasonStartYear}
                    numRows={getNumRows()}
                    totalPlaces={table.totalPlaces}
                    promotionPlaces={table.promotionPlaces}
                    playOffPlaces={table.playOffPlaces}
                    relegationPlaces={table.relegationPlaces}
                  />
                ))}
            </Table.Body>
          </Table>
          <PointDeductionSummary leagueTable={table.table} />
        </>
      )}
    </div>
  );
};

export { LeagueTable };
