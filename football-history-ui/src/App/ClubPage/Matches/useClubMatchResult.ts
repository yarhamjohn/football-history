import { Match } from "../../shared/useFetchLeagueMatches";
import { Team } from "../../shared/teamsSlice";

type MatchResult = "W" | "L" | "D";

const useClubMatchResult = (match: Match, selectedClub: Team): MatchResult => {
  const teamGoals = (match: Match) =>
    match.homeTeam.id === selectedClub.id ? match.homeTeam.goals : match.awayTeam.goals;
  const oppositionGoals = (match: Match) =>
    match.awayTeam.id === selectedClub.id ? match.homeTeam.goals : match.awayTeam.goals;

  const teamWon = teamGoals(match) > oppositionGoals(match);
  const teamLost = teamGoals(match) < oppositionGoals(match);

  return teamWon ? "W" : teamLost ? "L" : "D";
};

export { useClubMatchResult };
