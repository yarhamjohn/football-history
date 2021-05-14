import React, { FunctionComponent } from "react";
import { ResultsGrid } from "./ResultsGrid";
import { PlayOffs } from "./PlayOffs";
import { useFetchPlayOffMatches } from "../../shared/useFetchPlayOffMatches";
import { Loader } from "semantic-ui-react";
import { useFetchLeagueMatches } from "../../shared/useFetchLeagueMatches";
import { ErrorMessage } from "../../components/ErrorMessage";

const Matches: FunctionComponent<{ competitionId: number }> = ({ competitionId }) => {
  const playOffMatchesState = useFetchPlayOffMatches(competitionId);
  const leagueMatches = useFetchLeagueMatches({
    competitionId: competitionId,
  });

  if (leagueMatches.status === "UNLOADED") {
    return null;
  }

  if (leagueMatches.status === "LOADING") {
    return <Loader />;
  }

  if (leagueMatches.status === "LOAD_FAILED") {
    return <ErrorMessage header={"Something went wrong"} content={leagueMatches.error} />;
  }

  if (playOffMatchesState.status === "LOADING") {
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
        {playOffMatchesState.status === "LOAD_SUCCESSFUL" && (
          <PlayOffs matches={playOffMatchesState.data} />
        )}
        {<ResultsGrid matches={leagueMatches.data} />}
      </div>
    </div>
  );
};

export { Matches };
