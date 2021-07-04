import React, { FunctionComponent } from "react";
import { Checkbox } from "semantic-ui-react";

export type MatchFilterState = {
  home: boolean;
  away: boolean;
};

const MatchFilter: FunctionComponent<{
  matchFilterState: MatchFilterState;
  setMatchFilterState: (home: boolean, away: boolean) => void;
}> = ({ matchFilterState, setMatchFilterState }) => {
  return (
    <div style={{ marginBottom: "1rem", display: "flex", justifyContent: "center" }}>
      <Checkbox
        label="Home matches"
        checked={matchFilterState.home}
        onClick={() => setMatchFilterState(!matchFilterState.home, matchFilterState.away)}
        style={{ marginRight: "2rem" }}
      />
      <Checkbox
        label="Away matches"
        checked={matchFilterState.away}
        onClick={() => setMatchFilterState(matchFilterState.home, !matchFilterState.away)}
        style={{ marginRight: "2rem" }}
      />
    </div>
  );
};

export { MatchFilter };
