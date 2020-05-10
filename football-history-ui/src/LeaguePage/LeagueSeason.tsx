import React, { FunctionComponent, useEffect, useState } from "react";
import { LeagueTable } from "../components/LeagueTable/LeagueTable";
import { useSeasons } from "../hooks/useSeasons";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { ResultsGrid } from "../components/ResultsGrid";
import { PlayOffs } from "../components/PlayOffs";
import { useLeagueTable } from "../hooks/useLeagueTable";

const LeagueSeason: FunctionComponent<{ selectedTier: number }> = ({ selectedTier }) => {
  const { seasonsState } = useSeasons();
  const { leagueTableState, getLeagueTable } = useLeagueTable();
  const [selectedSeason, setSelectedSeason] = useState<number | undefined>(undefined);

  useEffect(() => {
    if (seasonsState.type !== "SEASONS_LOADED") {
      return;
    }

    setSelectedSeason(Math.max(...seasonsState.seasons.map((s) => s.startYear)));
  }, [selectedTier, seasonsState]);

  useEffect(() => {
    if (selectedSeason !== undefined) {
      getLeagueTable(selectedTier, selectedSeason);
    }
  }, [selectedTier, selectedSeason]);

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
      {leagueTableState.type === "LEAGUE_TABLE_LOADED" && selectedTier && (
        <>
          <h2 style={{ margin: 0 }}>{leagueTableState.leagueTable.name}</h2>
          <div
            style={{
              display: "grid",
              gridTemplateColumns: "repeat(auto-fit, minmax(1100px, 1fr))",
              gridGap: "1rem",
            }}
          >
            <LeagueTable table={leagueTableState.leagueTable} seasonStartYear={selectedSeason} />
            <div style={{ display: "grid", gridTemplateRows: "auto auto", gridGap: "1rem" }}>
              <PlayOffs tier={selectedTier} seasonStartYear={selectedSeason} />
              <ResultsGrid tier={selectedTier} seasonStartYear={selectedSeason} />
            </div>
          </div>
        </>
      )}
    </div>
  );
};

export { LeagueSeason };
