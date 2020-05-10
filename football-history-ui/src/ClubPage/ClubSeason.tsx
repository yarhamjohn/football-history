import React, { FunctionComponent, useEffect, useState } from "react";
import { useSeasons } from "../hooks/useSeasons";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { useLeagueTable } from "../hooks/useLeagueTable";
import { LeagueTable } from "../components/LeagueTable/LeagueTable";

const ClubSeason: FunctionComponent<{ selectedClub: string }> = ({ selectedClub }) => {
  const { seasonsState } = useSeasons();
  const { leagueTableState, getLeagueTableForTeam } = useLeagueTable();
  const [selectedSeason, setSelectedSeason] = useState<number | undefined>(undefined);

  useEffect(() => {
    if (seasonsState.type !== "SEASONS_LOADED") {
      return;
    }

    setSelectedSeason(Math.max(...seasonsState.seasons.map((s) => s.startYear)));
  }, [selectedClub, seasonsState]);

  useEffect(() => {
    if (selectedSeason !== undefined) {
      getLeagueTableForTeam(selectedClub, selectedSeason);
    }
  }, [selectedClub, selectedSeason]);

  if (seasonsState.type !== "SEASONS_LOADED") {
    return null;
  }

  return (
    <div style={{ display: "grid", gridGap: "1rem" }}>
      <SeasonFilter
        seasons={seasonsState.seasons}
        selectedSeason={selectedSeason}
        selectSeason={(startYear) => setSelectedSeason(startYear)}
      />
      {leagueTableState.type !== "LEAGUE_TABLE_LOADED" ? null : (
        <>
          <h2 style={{ margin: 0 }}>{leagueTableState.leagueTable.name}</h2>
          <div
            style={{
              display: "grid",
              gridTemplateColumns: "repeat(auto-fit, minmax(1100px, 1fr))",
              gridGap: "1rem",
            }}
          >
            <LeagueTable
              club={selectedClub}
              table={leagueTableState.leagueTable}
              seasonStartYear={selectedSeason}
            />
          </div>
        </>
      )}
    </div>
  );
};

export { ClubSeason };
