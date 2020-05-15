import React, { FunctionComponent } from "react";
import { LeagueTable } from "../components/LeagueTable/LeagueTable";
import { ResultsGrid } from "../components/ResultsGrid";
import { PlayOffs } from "../components/PlayOffs";
import { useLeague } from "../hooks/useLeagueTable";
import { usePlayOffMatches } from "../hooks/usePlayOffMatches";

const LeagueSeason: FunctionComponent<{ selectedTier: number; selectedSeason: number }> = ({
  selectedTier,
  selectedSeason,
}) => {
  const { leagueState } = useLeague(selectedSeason, undefined, selectedTier);
  const { playOffMatchesState } = usePlayOffMatches(selectedTier, selectedSeason);

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
        {playOffMatchesState.type === "PLAY_OFF_MATCHES_LOAD_SUCCEEDED" && (
          <PlayOffs matches={playOffMatchesState.matches} />
        )}
        <ResultsGrid tier={selectedTier} seasonStartYear={selectedSeason} />
      </div>
    </div>
  );
};

export { LeagueSeason };
