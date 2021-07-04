import { Match } from "../../../shared/useFetchLeagueMatches";

const useDrillDownForm = (matches: Match[], teamId: number) =>
  matches
    .sort((a, b) => b.matchDate.valueOf() - a.matchDate.valueOf())
    .map((m) =>
      m.homeTeam.id === teamId && m.homeTeam.goals > m.awayTeam.goals
        ? "W"
        : m.awayTeam.id === teamId && m.awayTeam.goals > m.homeTeam.goals
        ? "W"
        : m.homeTeam.goals === m.awayTeam.goals
        ? "D"
        : "L"
    );

export { useDrillDownForm };
