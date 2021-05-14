import { useApi } from "./useApi";
import { useFetch } from "./useFetch";
import { Season } from "./useFetchSeasons";

export type CompetitionRules = {
  pointsForWin: number;
  totalPlaces: number;
  promotionPlaces: number;
  relegationPlaces: number;
  playOffPlaces: number;
  relegationPlayOffPlaces: number;
  reElectionPlaces: number;
  failedReElectionPosition: number | null;
};

export type Competition = {
  id: number;
  name: string;
  season: Season;
  level: string;
  comment: string | null;
  rules: CompetitionRules;
};

const useFetchCompetitions = (seasonId: number) => {
  const api = useApi();

  const url = `${api}/api/v2/competitions/season/${seasonId}`;
  const result = useFetch(url);

  return result.status === "LOAD_SUCCESSFUL"
    ? { ...result, data: result.data as Competition[] }
    : result;
};

export { useFetchCompetitions };
