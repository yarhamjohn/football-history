import { useApi } from "./useApi";
import { useFetch } from "./useFetch";

export type Team = {
  id: number;
  name: string;
  abbreviation: string;
  notes: string | null;
};

const useFetchClubs = () => {
  const api = useApi();

  const url = `${api}/api/v2/teams`;
  const result = useFetch(url);

  return result.status === "LOAD_SUCCESSFUL" ? { ...result, data: result.data as Team[] } : result;
};

export { useFetchClubs };
