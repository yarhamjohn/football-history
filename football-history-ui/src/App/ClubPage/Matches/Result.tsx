import React, { FunctionComponent } from "react";
import { Match } from "../../shared/useFetchLeagueMatches";
import { Card } from "semantic-ui-react";
import { useClubMatchResult } from "./useClubMatchResult";
import { Team } from "../../shared/teamsSlice";

const MatchResult: FunctionComponent<{
  match: Match;
  selectedClub: Team;
}> = ({ match, selectedClub }) => {
  const result = useClubMatchResult(match, selectedClub);

  return (
    <Card
      raised
      color={result === "W" ? "green" : result === "L" ? "pink" : "olive"}
      style={{ background: "rgba(0,0,0,.03)", width: "100%", margin: "5px" }}
    >
      <Card.Content
        style={{
          display: "flex",
          alignItems: "center",
          justifyContent: "space-between",
          padding: "0 1rem",
        }}
      >
        <div style={{ display: "flex", justifyContent: "space-between", width: "30%" }}>
          <h2
            style={{
              fontSize: "20px",
              margin: "0 1rem 0 0",
              color: result === "W" ? "#75B266" : result === "L" ? "#B26694" : "#BFA67F",
            }}
          >
            {result}
          </h2>
          <div style={{ fontWeight: "bold" }}>{new Date(match.matchDate).toDateString()}</div>
        </div>
        <div>
          {match.homeTeam.name} {match.homeTeam.goals} - {match.awayTeam.goals}{" "}
          {match.awayTeam.name}
        </div>
      </Card.Content>
    </Card>
  );
};

export { MatchResult };
