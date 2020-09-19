import React, { FunctionComponent } from "react";
import { Table } from "semantic-ui-react";
import { LeagueTableRow } from "./LeagueTableRow";
import { PointDeductionSummary } from "./PointDeductionSummary";
import { useLeague } from "../../hooks/useLeagueTable";

const LeagueTable: FunctionComponent<{
  selectedSeason: number;
  selectedClub?: string;
  selectedTier?: number;
}> = ({ selectedSeason, selectedClub, selectedTier }) => {
  const { leagueState } = useLeague(selectedSeason, selectedClub, selectedTier);

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
                {selectedSeason == 2019 && (leagueState.league.tier == 3 || leagueState.league.tier == 4) ? <Table.HeaderCell>PPG</Table.HeaderCell> : null}
                <Table.HeaderCell></Table.HeaderCell>
              </Table.Row>
            </Table.Header>
            <Table.Body>
              {selectedSeason &&
                leagueState.league.table.map((r) => (
                  <LeagueTableRow
                    key={r.position}
                    row={r}
                    club={selectedClub}
                    seasonStartYear={selectedSeason}
                    numRows={getNumRows()}
                    totalPlaces={leagueState.league.totalPlaces}
                    promotionPlaces={leagueState.league.promotionPlaces}
                    playOffPlaces={leagueState.league.playOffPlaces}
                    relegationPlaces={leagueState.league.relegationPlaces}
                    tier={leagueState.league.tier}
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
