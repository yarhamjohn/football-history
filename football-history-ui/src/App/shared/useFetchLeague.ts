import { useApi } from "./useApi";
import { Competition } from "./useFetchCompetitions";
import { useFetch } from "./useFetch";

export type Row = {
  position: number;
  teamId: number;
  team: string;
  played: number;
  won: number;
  drawn: number;
  lost: number;
  goalsFor: number;
  goalsAgainst: number;
  goalDifference: number;
  goalAverage: number;
  points: number;
  pointsPerGame: number;
  pointsDeducted: number;
  pointsDeductionReason: string | null;
  status: string | null;
};

export type League = {
  table: Row[];
  competition: Competition;
};

type FetchLeagueProps =
  | {
      competitionId: number;
    }
  | {
      teamId: number;
      seasonId: number;
    };

const useFetchLeague = (props: FetchLeagueProps) => {
  const api = useApi();

  const url =
    "competitionId" in props
      ? `${api}/api/v2/league-table/competition/${props.competitionId}`
      : `${api}/api/v2/league-table/season/${props.seasonId}/team/${props.teamId}`;

  const result = useFetch(url);
  return result.status === "LOAD_SUCCESSFUL" ? { ...result, data: result.data as League } : result;
};

export { useFetchLeague };
