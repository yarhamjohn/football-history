import React, { FunctionComponent, useEffect } from "react";
import { useLeagueTable } from "./useLeagueTable";
import { Table } from "semantic-ui-react";
import { LeagueTableRow } from "./LeagueTableRow";
import { PointDeductionSummary } from "./PointDeductionSummary";

const LeagueTable: FunctionComponent<{
  club: string;
  seasonStartYear: number | undefined;
  style?: React.CSSProperties;
}> = ({ club, seasonStartYear, style }) => {
  const { leagueTable, getLeagueTable } = useLeagueTable();

  useEffect(() => {
    if (seasonStartYear !== undefined) {
      getLeagueTable(club, seasonStartYear);
    }
  }, [club, seasonStartYear]);

  function getNumRows() {
    if (leagueTable && leagueTable.table) {
      return leagueTable.table.length;
    }

    return 0;
  }

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
        <>
          <h2>{leagueTable.name}</h2>
          <Table basic compact collapsing>
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
              {leagueTable &&
                seasonStartYear &&
                leagueTable.table.map((r) => (
                  <LeagueTableRow
                    key={r.position}
                    row={r}
                    club={club}
                    seasonStartYear={seasonStartYear}
                    numRows={getNumRows()}
                    totalPlaces={leagueTable.totalPlaces}
                    promotionPlaces={leagueTable.promotionPlaces}
                    playOffPlaces={leagueTable.playOffPlaces}
                    relegationPlaces={leagueTable.relegationPlaces}
                  />
                ))}
            </Table.Body>
          </Table>
          <PointDeductionSummary leagueTable={leagueTable.table} />
        </>
      )}
    </div>
  );
};

export { LeagueTable };