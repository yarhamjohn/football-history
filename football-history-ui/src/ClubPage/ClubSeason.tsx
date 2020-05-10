import React, { FunctionComponent, useEffect, useState } from "react";
import { Club } from "../hooks/useClubs";
import { LeagueTable } from "../components/LeagueTable/LeagueTable";
import { useSeasons } from "../hooks/useSeasons";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { PlayOffs } from "../components/PlayOffs";
import { useTiers } from "../hooks/useTiers";
import { ResultsGrid } from "../components/ResultsGrid";

const ClubSeason: FunctionComponent<{ selectedClub: Club }> = ({ selectedClub }) => {
  const { tier, getTier } = useTiers();
  const { seasons } = useSeasons();
  const [selectedSeason, setSelectedSeason] = useState<number | undefined>(undefined);

  useEffect(() => {
    if (seasons === undefined || selectedClub === undefined) {
      return;
    }

    setSelectedSeason(Math.max(...seasons.map((s) => s.startYear)));
  }, [selectedClub, seasons]);

  useEffect(() => {
    if (selectedSeason !== undefined) {
      getTier(selectedClub.name, selectedSeason);
    }
  }, [selectedClub, selectedSeason]);

  const getDivisionName = () => {
    if (seasons !== undefined) {
      const season = seasons.filter((s) => s.startYear === selectedSeason);

      if (season.length !== 1) {
        return;
      }

      const division = season[0].divisions.filter((d) => d.tier === tier);
      if (division.length !== 1) {
        return null;
      }

      return division[0].name;
    }
  };

  if (seasons === undefined) {
    return null;
  }

  return (
    <div style={{ display: "grid", gridGap: "1rem" }}>
      <SeasonFilter
        seasons={seasons}
        selectedSeason={selectedSeason}
        setSelectedSeason={(startYear) => setSelectedSeason(startYear)}
      />
      <h2 style={{ margin: 0 }}>{getDivisionName()}</h2>
      <div
        style={{
          display: "grid",
          gridTemplateColumns: "repeat(auto-fit, minmax(1100px, 1fr))",
          gridGap: "1rem",
        }}
      >
        <LeagueTable club={selectedClub.name} seasonStartYear={selectedSeason} />
      </div>
    </div>
  );
};

export { ClubSeason };