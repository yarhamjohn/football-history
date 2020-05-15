import React, { FunctionComponent, useEffect, useState } from "react";
import { SeasonState } from "../hooks/useSeasons";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { ClubSeason } from "../ClubPage/ClubSeason";
import { LeagueSeason } from "../LeaguePage/LeagueSeason";

const Season: FunctionComponent<{
  selectedClub?: string;
  selectedTier?: number;
  seasonState: SeasonState;
}> = ({ selectedClub, selectedTier, seasonState }) => {
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

  const season =
    selectedSeason && selectedTier ? (
      <LeagueSeason selectedSeason={selectedSeason} selectedTier={selectedTier} />
    ) : selectedSeason && selectedClub ? (
      <ClubSeason selectedSeason={selectedSeason} selectedClub={selectedClub} />
    ) : null;

  return (
    <div style={{ display: "grid", gridGap: "1rem" }}>
      <SeasonFilter
        seasons={seasonState.seasons}
        selectedSeason={selectedSeason}
        selectSeason={(startYear) => setSelectedSeason(startYear)}
      />
      {season}
    </div>
  );
};

export { Season };
