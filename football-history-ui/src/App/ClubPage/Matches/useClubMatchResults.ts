import _ from "lodash";
import { Match } from "../../shared/useFetchLeagueMatches";

const useClubMatchResults = (matches: Match[]) => {
  const dictionary = _.groupBy(matches, (m) => new Date(m.matchDate).getMonth());
  const matchesGroupedByMonth = _.map(dictionary, (v, _) => v).sort(
    (a, b) =>
      Math.min(...a.map((m) => new Date(m.matchDate).valueOf())) -
      Math.min(...b.map((m) => new Date(m.matchDate).valueOf()))
  );

  return { matchesGroupedByMonth };
};

export { useClubMatchResults };
