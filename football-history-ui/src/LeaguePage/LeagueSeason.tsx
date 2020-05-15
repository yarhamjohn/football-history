import React, { FunctionComponent, useEffect, useState } from "react";
import { LeagueTable } from "../components/LeagueTable/LeagueTable";
import { SeasonState, useSeasons } from "../hooks/useSeasons";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { ResultsGrid } from "../components/ResultsGrid";
import { PlayOffs } from "../components/PlayOffs";

const LeagueSeason: FunctionComponent<{ selectedTier: number; seasonState: SeasonState }> = ({
  selectedTier,
  seasonState,
}) => {
  const [selectedSeason, setSelectedSeason] = useState<number | undefined>(undefined);

  useEffect(() => {
    if (seasonState.type !== "SEASONS_LOAD_SUCCEEDED") {
      return;
    }

    setSelectedSeason(Math.max(...seasonState.seasons.map((s) => s.startYear)));
  }, [selectedTier, seasonState]);

  if (seasonState.type !== "SEASONS_LOAD_SUCCEEDED") {
    return null;
  }

  return (
    <div style={{ display: "grid", gridGap: "1rem" }}>
      <SeasonFilter
        seasons={seasonState.seasons}
        selectedSeason={selectedSeason}
        selectSeason={(startYear) => setSelectedSeason(startYear)}
      />
      {selectedTier && selectedSeason && (
        <>
          <div
            style={{
              display: "grid",
              gridTemplateColumns: "repeat(auto-fit, minmax(1100px, 1fr))",
              gridGap: "1rem",
            }}
          >
            <LeagueTable seasonStartYear={selectedSeason} tier={selectedTier} />
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
