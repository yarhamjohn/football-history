import React, { FunctionComponent } from "react";
import { Match } from "../../../shared/useFetchLeagueMatches";
import { PlayOffSemiFinal } from "./PlayOffSemiFinal";
import { PlayOffFinalOneLegged } from "./PlayOffFinalOneLegged";
import { PlayOffFinalTwoLegged } from "./PlayOffFinalTwoLegged";
import { PlayOffFinalReplayed } from "./PlayOffFinalReplayed";

const PlayOffs: FunctionComponent<{
  matches: Match[];
}> = ({ matches }) => {
  if (matches.length === 0) {
    return null;
  }

  const semiFinals = matches.filter((m) => m.rules.stage === "Semi-Final");
  const teams = semiFinals.map((m) => m.homeTeam.name);
  const semiFinalOne = semiFinals.filter(
    (m) => m.homeTeam.name === teams[0] || m.awayTeam.name === teams[0]
  );
  const semiFinalTwo = semiFinals.filter(
    (m) => m.homeTeam.name !== teams[0] && m.awayTeam.name !== teams[0]
  );
  const finalMatches = matches.filter((m) => m.rules.stage === "Final");
  return (
    <div
      style={{
        display: "grid",
        gridTemplateRows: "auto auto 1rem auto auto",
        gridTemplateColumns: "auto auto 1rem auto auto",
        gridTemplateAreas:
          "'. one . . .' '. one . final .' '. . . final .' '. two . final .' '. two . . .'",
      }}
    >
      {<PlayOffSemiFinal semiFinal={semiFinalOne} style={{ gridArea: "one" }} />}
      {<PlayOffSemiFinal semiFinal={semiFinalTwo} style={{ gridArea: "two", marginTop: 0 }} />}
      {finalMatches.length === 1 ? (
        <PlayOffFinalOneLegged final={finalMatches[0]} style={{ gridArea: "final" }} />
      ) : finalMatches.length === 2 ? (
        <PlayOffFinalTwoLegged final={finalMatches} style={{ gridArea: "final" }} />
      ) : (
        <PlayOffFinalReplayed final={finalMatches} style={{ gridArea: "final" }} />
      )}
    </div>
  );
};

export { PlayOffs };
