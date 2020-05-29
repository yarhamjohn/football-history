import React, { FunctionComponent } from "react";
import { LeagueTable } from "../components/LeagueTable/LeagueTable";
import { ResultsGrid } from "../components/ResultsGrid";
import { PlayOffs } from "../components/PlayOffs";
import { useLeague } from "../hooks/useLeagueTable";
import { usePlayOffMatches } from "../hooks/usePlayOffMatches";
import { Loader } from "semantic-ui-react";
import { useLeagueMatches } from "../hooks/useLeagueMatches";

const LeagueSeason: FunctionComponent<{ selectedTier: number; selectedSeason: number }> = ({
  selectedTier,
  selectedSeason,
}) => {
  const { leagueState } = useLeague(selectedSeason, undefined, selectedTier);
  const { playOffMatchesState } = usePlayOffMatches(selectedTier, selectedSeason);
  const { leagueMatchesState } = useLeagueMatches(selectedSeason, undefined, selectedTier);

  if (
    leagueState.type === "LEAGUE_LOADING" ||
    playOffMatchesState.type === "PLAY_OFF_MATCHES_LOADING" ||
    leagueMatchesState.type === "LEAGUE_MATCHES_LOADING"
  ) {
    return <Loader active />;
  }

  return (
    <div
      style={{
        display: "grid",
        gridTemplateRows: "auto auto",
        gridGap: "1rem",
      }}
    >
      <LeagueTable selectedSeason={selectedSeason} selectedTier={selectedTier} />
      <div style={{ display: "grid", gridTemplateRows: "auto auto", gridGap: "1rem" }}>
        {playOffMatchesState.type === "PLAY_OFF_MATCHES_LOAD_SUCCEEDED" && (
          <PlayOffs matches={playOffMatchesState.matches} />
        )}
        {leagueMatchesState.type === "LEAGUE_MATCHES_LOAD_SUCCEEDED" && (
          <ResultsGrid matches={leagueMatchesState.matches} />
        )}
      </div>
    </div>
  );
};

export { LeagueSeason };
