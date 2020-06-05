import React, { FunctionComponent } from "react";
import { LeagueMatch, useLeagueMatches } from "../hooks/useLeagueMatches";
import { Message, Item, Divider, Card } from "semantic-ui-react";
import _ from "lodash";

const Matches: FunctionComponent<{ selectedSeason: number; selectedClub: string }> = ({
  selectedSeason,
  selectedClub,
}) => {
  const { leagueMatchesState } = useLeagueMatches(selectedSeason, selectedClub);

  if (leagueMatchesState.type !== "LEAGUE_MATCHES_LOAD_SUCCEEDED") {
    return null;
  }

  let dictionary = _.groupBy(leagueMatchesState.matches, (m) => new Date(m.date).getMonth());
  let array = _.map(dictionary, (v, k) => v);
  return (
    <div>
      {array.map((month) => (
        <>
          <Card.Group relaxed style={{ display: "flex" }}>
            {month.map((match) => (
              <Card>
                <Card.Content>{match.date}</Card.Content>
                <div
                  style={{ display: "flex", justifyContent: "space-around", marginBottom: "1rem" }}
                >
                  <Card.Content
                    style={{ display: "flex", flexDirection: "column", alignItems: "center" }}
                  >
                    <p>{match.homeTeam}</p>
                    <p>{match.homeGoals}</p>
                  </Card.Content>
                  <Card.Content style={{ display: "flex", flexDirection: "column" }}>
                    <p>{match.awayTeam}</p>
                    <p>{match.awayGoals}</p>
                  </Card.Content>
                </div>
              </Card>
            ))}
          </Card.Group>
          <Divider />
        </>
      ))}
    </div>
  );
  // <Message info>There were no league matches</Message>
  // );
};

export { Matches };
