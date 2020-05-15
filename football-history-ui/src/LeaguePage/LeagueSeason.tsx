import React, { FunctionComponent } from "react";
import { LeagueTable } from "../components/LeagueTable/LeagueTable";
import { ResultsGrid } from "../components/ResultsGrid";
import { PlayOffs } from "../components/PlayOffs";
import { useLeague } from "../hooks/useLeagueTable";

const LeagueSeason: FunctionComponent<{ selectedTier: number; selectedSeason: number }> = ({
  selectedTier,
  selectedSeason,
}) => {
  const { leagueState } = useLeague(selectedSeason, undefined, selectedTier);

  return (
    <div
      style={{
        display: "grid",
        gridTemplateColumns: "repeat(auto-fit, minmax(1100px, 1fr))",
        gridGap: "1rem",
      }}
    >
      <LeagueTable leagueState={leagueState} seasonStartYear={selectedSeason} />
      <div style={{ display: "grid", gridTemplateRows: "auto auto", gridGap: "1rem" }}>
        <PlayOffs tier={selectedTier} seasonStartYear={selectedSeason} />
        <ResultsGrid tier={selectedTier} seasonStartYear={selectedSeason} />
      </div>
    </div>
  );
};

export { LeagueSeason };
