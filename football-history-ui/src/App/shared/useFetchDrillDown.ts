import { useFetchLeaguePositions } from "./useFetchLeaguePositions";
import { useFetchLeagueMatches } from "./useFetchLeagueMatches";

const useFetchDrillDown = (competitionId: number, teamId: number) => {
  const leaguePositions = useFetchLeaguePositions(competitionId, teamId);
  const leagueMatches = useFetchLeagueMatches({
    competitionId,
    teamId,
  });

  return {
    leaguePositions,
    leagueMatches,
  };
};

export { useFetchDrillDown };
