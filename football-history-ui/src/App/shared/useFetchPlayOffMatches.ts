import { useApi } from "./useApi";
import { useFetch } from "./useFetch";
import { Match } from "./useFetchLeagueMatches";

const useFetchPlayOffMatches = (competitionId: number) => {
  const api = useApi();

  const url = `${api}/api/v2/matches?competitionId=${competitionId}&type=PlayOff`;

  const result = useFetch(url);
  return result.status === "LOAD_SUCCESSFUL" ? { ...result, data: result.data as Match[] } : result;
};

export { useFetchPlayOffMatches };
