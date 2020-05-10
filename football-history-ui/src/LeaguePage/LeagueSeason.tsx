import React, { FunctionComponent, useEffect, useState } from "react";
import { LeagueTable } from "../components/LeagueTable/LeagueTable";
import { useSeasons } from "../hooks/useSeasons";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { ResultsGrid } from "../components/ResultsGrid";
import { PlayOffs } from "../components/PlayOffs";
import { useLeagueTable } from "../hooks/useLeagueTable";

const LeagueSeason: FunctionComponent<{ selectedTier: number | undefined }> = ({
  selectedTier,
}) => {
  const { seasons } = useSeasons();
  const [selectedSeason, setSelectedSeason] = useState<number | undefined>(undefined);
  const { leagueTableState, getLeagueTable } = useLeagueTable();
  useEffect(() => {
    if (selectedSeason !== undefined && selectedTier !== undefined) {
      getLeagueTable(selectedTier, selectedSeason);
    }
  }, [selectedTier, selectedSeason]);
  useEffect(() => {
    if (seasons === undefined || selectedTier === undefined) {
      return;
    }

    setSelectedSeason(Math.max(...seasons.map((s) => s.startYear)));
  }, [selectedTier, seasons]);

  const getDivisionName = () => {
    if (seasons !== undefined) {
      const season = seasons.filter((s) => s.startYear === selectedSeason);

      if (season.length !== 1) {
        return;
      }

      const division = season[0].divisions.filter((d) => d.tier === selectedTier);
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
      {leagueTableState.type === "LEAGUE_TABLE_LOADED" && selectedTier && (
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
      )}
    </div>
  );
};

export { LeagueSeason };
