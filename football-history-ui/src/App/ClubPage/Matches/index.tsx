import React, { FunctionComponent, useState } from "react";
import { useFetchLeagueMatches } from "../../shared/useFetchLeagueMatches";
import { Divider, Loader } from "semantic-ui-react";
import { Team } from "../../shared/useFetchClubs";
import { ErrorMessage } from "../../components/ErrorMessage";
import { MatchResults } from "./Results";
import { MatchFilter, MatchFilterState } from "./Filter";
import { Season } from "../../shared/seasonsSlice";

const Matches: FunctionComponent<{ selectedSeason: Season; selectedClub: Team }> = ({
  selectedSeason,
  selectedClub,
}) => {
  const result = useFetchLeagueMatches({
    seasonId: selectedSeason.id,
    teamId: selectedClub.id,
  });

  const [matchFilterState, setMatchFilterState] = useState<MatchFilterState>({
    home: true,
    away: true,
  });

  if (result.status === "UNLOADED") {
    return null;
  }

  if (result.status === "LOADING") {
    return <Loader />;
  }

  if (result.status === "LOAD_FAILED") {
    return <ErrorMessage header={"Something went wrong"} content={result.error} />;
  }

  const matches = result.data.filter(
    (m) =>
      (matchFilterState.home && m.homeTeam.id === selectedClub.id) ||
      (matchFilterState.away && m.awayTeam.id === selectedClub.id)
  );

  return (
    <div>
      <MatchFilter
        matchFilterState={matchFilterState}
        setMatchFilterState={(home, away) => setMatchFilterState({ home, away })}
      />
      <Divider />
      <MatchResults matches={matches} selectedClub={selectedClub} />
    </div>
  );
};

export { Matches };
