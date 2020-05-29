import React, { FunctionComponent } from "react";
import { ResultsGrid } from "./ResultsGrid";
import { PlayOffs } from "./PlayOffs";
import { usePlayOffMatches } from "../hooks/usePlayOffMatches";
import { Loader } from "semantic-ui-react";
import { useLeagueMatches } from "../hooks/useLeagueMatches";

const Matches: FunctionComponent<{ selectedTier: number; selectedSeason: number }> = ({
  selectedTier,
  selectedSeason,
}) => {
  const { playOffMatchesState } = usePlayOffMatches(selectedTier, selectedSeason);
  const { leagueMatchesState } = useLeagueMatches(selectedSeason, undefined, selectedTier);

  if (
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

export { Matches };
