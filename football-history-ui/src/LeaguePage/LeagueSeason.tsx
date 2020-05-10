import React, { FunctionComponent, useEffect, useState } from "react";
import { LeagueTable } from "../components/LeagueTable/LeagueTable";
import { Season, useSeasons } from "../hooks/useSeasons";
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

  const getDivisionName = (seasons: Season[]) => {
    const season = seasons.filter((s) => s.startYear === selectedSeason);

    if (season.length !== 1) {
      return;
    }

    const division = season[0].divisions.filter((d) => d.tier === selectedTier);
    if (division.length !== 1) {
      return null;
    }

    return division[0].name;
  };

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
      <h2 style={{ margin: 0 }}>{getDivisionName(seasonsState.seasons)}</h2>
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
