import { Match } from "../../../shared/useFetchLeagueMatches";

const useDrillDownForm = (matches: Match[], clubId: number) =>
  matches
    .sort((a, b) => b.matchDate.valueOf() - a.matchDate.valueOf())
    .map((m) =>
      m.homeTeam.id === clubId && m.homeTeam.goals > m.awayTeam.goals
        ? "W"
        : m.awayTeam.id === clubId && m.awayTeam.goals > m.homeTeam.goals
        ? "W"
        : m.homeTeam.goals === m.awayTeam.goals
        ? "D"
        : "L"
    );

export { useDrillDownForm };
