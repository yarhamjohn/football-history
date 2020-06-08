import React, { FunctionComponent, useState } from "react";
import { LeagueMatch, useLeagueMatches } from "../hooks/useLeagueMatches";
import { Divider, Card, Message, Checkbox } from "semantic-ui-react";
import _ from "lodash";

const Matches: FunctionComponent<{ selectedSeason: number; selectedClub: string }> = ({
  selectedSeason,
  selectedClub,
}) => {
  const { leagueMatchesState } = useLeagueMatches(selectedSeason, selectedClub);
  const [matches, setMatches] = useState<{ homeMatches: boolean; awayMatches: boolean }>({
    homeMatches: true,
    awayMatches: true,
  });

  if (leagueMatchesState.type !== "LEAGUE_MATCHES_LOAD_SUCCEEDED") {
    return null;
  }

  const teamWon = (match: LeagueMatch) => {
    const teamGoals = match.homeTeam === selectedClub ? match.homeGoals : match.awayGoals;
    const oppositionGoals = match.awayTeam === selectedClub ? match.homeGoals : match.awayGoals;

    return teamGoals > oppositionGoals;
  };

  const teamLost = (match: LeagueMatch) => {
    const teamGoals = match.homeTeam === selectedClub ? match.homeGoals : match.awayGoals;
    const oppositionGoals = match.awayTeam === selectedClub ? match.homeGoals : match.awayGoals;

    return teamGoals < oppositionGoals;
  };

  let collection = leagueMatchesState.matches.filter(
    (m) =>
      (matches.homeMatches && m.homeTeam === selectedClub) ||
      (matches.awayMatches && m.awayTeam === selectedClub)
  );
  let dictionary = _.groupBy(collection, (m) => new Date(m.date).getMonth());
  let array = _.map(dictionary, (v, k) => v).sort(
    (a, b) =>
      Math.min(...a.map((m) => new Date(m.date).valueOf())) -
      Math.min(...b.map((m) => new Date(m.date).valueOf()))
  );
  return (
    <div>
      <MatchesFilter
        matches={matches}
        setMatches={(homeMatches, awayMatches) => setMatches({ homeMatches, awayMatches })}
      />
      <Divider />
      {leagueMatchesState.matches.length > 0 ? (
        array.map((month) => (
          <>
            <Card.Group style={{ margin: 0 }}>
              {month.map((match) => (
                <Card
                  raised
                  color={teamWon(match) ? "green" : teamLost(match) ? "pink" : "olive"}
                  style={{ background: "rgba(0,0,0,.03)", width: "fit-content" }}
                >
                  <Card.Content
                    style={{ display: "flex", alignItems: "center", padding: "0 1rem" }}
                  >
                    <h2
                      style={{
                        fontSize: "40px",
                        margin: "0 1rem 0 0",
                        color: teamWon(match) ? "#75B266" : teamLost(match) ? "#B26694" : "#BFA67F",
                      }}
                    >
                      {teamWon(match) ? "W" : teamLost(match) ? "L" : "D"}
                    </h2>
                    <div style={{ display: "flex", flexDirection: "column" }}>
                      <Card.Header style={{ fontWeight: "bold" }}>
                        {new Date(match.date).toDateString()}
                      </Card.Header>
                      <Card.Description>
                        {match.homeTeam} {match.homeGoals} - {match.awayGoals} {match.awayTeam}
                      </Card.Description>
                    </div>
                  </Card.Content>
                </Card>
              ))}
            </Card.Group>
            <Divider style={{ margin: 0 }} />
          </>
        ))
      ) : (
        <Message info>
          {selectedClub} did not feature in the leagues during the {selectedSeason} -{" "}
          {selectedSeason + 1} season.
        </Message>
      )}
    </div>
  );
};

const MatchesFilter: FunctionComponent<{
  matches: { homeMatches: boolean; awayMatches: boolean };
  setMatches: (homeMatches: boolean, awayMatches: boolean) => void;
}> = ({ matches, setMatches }) => {
  return (
    <div style={{ marginBottom: "1rem" }}>
      <Checkbox
        label="Home matches"
        checked={matches.homeMatches}
        onClick={() => setMatches(!matches.homeMatches, matches.awayMatches)}
        style={{ marginRight: "2rem" }}
      />
      <Checkbox
        label="Away matches"
        checked={matches.awayMatches}
        onClick={() => setMatches(matches.homeMatches, !matches.awayMatches)}
        style={{ marginRight: "2rem" }}
      />
    </div>
  );
};

export { Matches };
