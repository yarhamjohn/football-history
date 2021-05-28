import { useApi } from "./useApi";
import { useFetch } from "./useFetch";

interface MatchCompetition {
  id: number;
  name: string;
  startYear: number;
  endYear: number;
  level: string;
}

interface MatchRules {
  type: string;
  stage: string;
  extraTime: boolean;
  penalties: boolean;
  numLegs: number;
  awayGoals: boolean;
  replays: boolean;
}

interface MatchTeam {
  id: number;
  name: string;
  abbreviation: string;
  goals: number;
  goalsExtraTime: number;
  penaltiesTaken: number;
  penaltiesScored: number;
}

export interface Match {
  id: number;
  matchDate: Date;
  competition: MatchCompetition;
  rules: MatchRules;
  homeTeam: MatchTeam;
  awayTeam: MatchTeam;
}

type FetchLeagueMatchesProps =
  | {
      competitionId: number;
    }
  | {
      teamId: number;
      seasonId: number;
    }
  | {
      teamId: number;
      competitionId: number;
    };

const useFetchLeagueMatches = (props: FetchLeagueMatchesProps) => {
  const api = useApi();

  const url =
    "seasonId" in props
      ? `${api}/api/v2/matches?teamId=${props.teamId}&seasonId=${props.seasonId}&type=League`
      : "teamId" in props
      ? `${api}/api/v2/matches?teamId=${props.teamId}&competitionId=${props.competitionId}&type=League`
      : `${api}/api/v2/matches?competitionId=${props.competitionId}&type=League`;

  const result = useFetch(url);
  return result.status === "LOAD_SUCCESSFUL" ? { ...result, data: result.data as Match[] } : result;
};

export { useFetchLeagueMatches };
