import { Match } from "../../shared/useFetchLeagueMatches";
import { Team } from "../../shared/teamsSlice";

type MatchResult = "W" | "L" | "D";

const useTeamMatchResult = (match: Match, selectedTeam: Team): MatchResult => {
  const teamGoals = (match: Match) =>
    match.homeTeam.id === selectedTeam.id ? match.homeTeam.goals : match.awayTeam.goals;
  const oppositionGoals = (match: Match) =>
    match.awayTeam.id === selectedTeam.id ? match.homeTeam.goals : match.awayTeam.goals;

  const teamWon = teamGoals(match) > oppositionGoals(match);
  const teamLost = teamGoals(match) < oppositionGoals(match);

  return teamWon ? "W" : teamLost ? "L" : "D";
};

export { useTeamMatchResult };
