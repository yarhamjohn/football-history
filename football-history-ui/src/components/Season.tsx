import React, { FunctionComponent, useEffect, useState } from "react";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { ClubSeason } from "../ClubPage/ClubSeason";
import { LeagueSeason } from "../LeaguePage/LeagueSeason";
import { Season as SeasonType } from "../hooks/useSeasons";

const Season: FunctionComponent<{
  selectedClub?: string;
  selectedTier?: number;
  seasons: SeasonType[];
}> = ({ selectedClub, selectedTier, seasons }) => {
  const [selectedSeason, setSelectedSeason] = useState<number | undefined>(undefined);

  useEffect(() => {
    setSelectedSeason(Math.max(...seasons.map((s) => s.startYear)));
  }, [selectedTier, seasons]);

  const season =
    selectedSeason && selectedTier ? (
      <LeagueSeason selectedSeason={selectedSeason} selectedTier={selectedTier} />
    ) : selectedSeason && selectedClub ? (
      <ClubSeason selectedSeason={selectedSeason} selectedClub={selectedClub} />
    ) : null;

  return (
    <div style={{ display: "grid", gridGap: "1rem" }}>
      <SeasonFilter
        seasons={seasons}
        selectedSeason={selectedSeason}
        selectSeason={(startYear) => setSelectedSeason(startYear)}
      />
      {season}
    </div>
  );
};

export { Season };
