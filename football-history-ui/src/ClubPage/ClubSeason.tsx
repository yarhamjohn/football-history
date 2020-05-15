import React, { FunctionComponent, useEffect, useState } from "react";
import { SeasonState } from "../hooks/useSeasons";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { LeagueTable } from "../components/LeagueTable/LeagueTable";

const ClubSeason: FunctionComponent<{ selectedClub: string; seasonState: SeasonState }> = ({
  selectedClub,
  seasonState,
}) => {
  const [selectedSeason, setSelectedSeason] = useState<number | undefined>(undefined);

  useEffect(() => {
    if (seasonState.type !== "SEASONS_LOAD_SUCCEEDED") {
      return;
    }

    setSelectedSeason(Math.max(...seasonState.seasons.map((s) => s.startYear)));
  }, [selectedClub, seasonState]);

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
      {selectedSeason && (
        <div
          style={{
            display: "grid",
            gridTemplateColumns: "repeat(auto-fit, minmax(1100px, 1fr))",
            gridGap: "1rem",
          }}
        >
          <LeagueTable seasonStartYear={selectedSeason} club={selectedClub} />
        </div>
      )}
    </div>
  );
};

export { ClubSeason };
