import React, { FunctionComponent } from "react";
import { Match } from "../../shared/useFetchLeagueMatches";
import { Card, Divider, Message } from "semantic-ui-react";
import { MatchResult } from "./Result";
import { useTeamMatchResults } from "./useTeamMatchResults";
import { Team } from "../../shared/teamsSlice";

const MatchResults: FunctionComponent<{
  matches: Match[];
  selectedTeam: Team;
}> = ({ matches, selectedTeam }) => {
  const { matchesGroupedByMonth } = useTeamMatchResults(matches);

  if (matches.length === 0) {
    return <Message info>No matches found.</Message>;
  }

  return (
    <div>
      {matchesGroupedByMonth.map((monthOfMatches) => (
        <div key={`month - ${monthOfMatches[0].id}`}>
          <Card.Group style={{ margin: 0 }}>
            {monthOfMatches.map((match) => (
              <MatchResult key={match.id} match={match} selectedTeam={selectedTeam} />
            ))}
          </Card.Group>
          <Divider style={{ margin: "10px 100px" }} />
        </div>
      ))}
    </div>
  );
};

export { MatchResults };
