import React, { FunctionComponent, useEffect } from "react";
import { Table } from "semantic-ui-react";
import { LeagueTableRow } from "./LeagueTableRow";
import { PointDeductionSummary } from "./PointDeductionSummary";
import { useLeague } from "../../hooks/useLeagueTable";

const LeagueTable: FunctionComponent<{
  seasonStartYear: number;
  club?: string;
  tier?: number;
}> = ({ seasonStartYear, club, tier }) => {
  const { leagueState } = useLeague(seasonStartYear, club, tier);

  function getNumRows() {
    if (leagueState.type === "LEAGUE_LOAD_SUCCEEDED" && leagueState.league.table) {
      return leagueState.league.table.length;
    }

    return 0;
  }

  if (leagueState.type !== "LEAGUE_LOAD_SUCCEEDED") {
    return null;
  }

  return (
    <div>
      {leagueState.league.table === null ? (
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
              {seasonStartYear &&
                leagueState.league.table.map((r) => (
                  <LeagueTableRow
                    key={r.position}
                    row={r}
                    club={club}
                    seasonStartYear={seasonStartYear}
                    numRows={getNumRows()}
                    totalPlaces={leagueState.league.totalPlaces}
                    promotionPlaces={leagueState.league.promotionPlaces}
                    playOffPlaces={leagueState.league.playOffPlaces}
                    relegationPlaces={leagueState.league.relegationPlaces}
                  />
                ))}
            </Table.Body>
          </Table>
          <PointDeductionSummary leagueTable={leagueState.league.table} />
        </>
      )}
    </div>
  );
};

export { LeagueTable };
