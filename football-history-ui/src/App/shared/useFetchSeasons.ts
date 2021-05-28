import { useApi } from "./useApi";
import { useFetch } from "./useFetch";

export type Season = {
  id: number;
  startYear: number;
  endYear: number;
};

const useFetchSeasons = () => {
  const api = useApi();

  const url = `${api}/api/v2/seasons`;
  const result = useFetch(url);

  return result.status === "LOAD_SUCCESSFUL"
    ? { ...result, data: result.data.filter((x: Season) => x.startYear >= 1958) } //TODO: Currently the UI is not configured to handle seasons pre-1958
    : result;
};

export { useFetchSeasons };
