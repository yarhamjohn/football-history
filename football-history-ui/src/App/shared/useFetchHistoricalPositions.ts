import { Reducer, useEffect, useReducer, useState } from "react";
import { useApi } from "./useApi";
import { HistoricalPositionRange } from "../components/HistoricalPositions";
import { Competition } from "./useFetchCompetitions";
import { Season } from "./seasonsSlice";

export type HistoricalPosition = {
  seasonId: number;
  seasonStartYear: number;
  competitions: Competition[];
  team: {
    competitionId: number;
    position: number;
    absolutePosition: number;
    status: string;
  } | null;
};

export type HistoricalPositionsState =
  | { type: "HISTORICAL_POSITIONS_UNLOADED" }
  | { type: "HISTORICAL_POSITIONS_LOADING"; positions: HistoricalPosition[] }
  | {
      type: "HISTORICAL_POSITIONS_LOADED";
      positions: HistoricalPosition[];
    }
  | { type: "HISTORICAL_POSITIONS_LOAD_FAILED"; error: string };

export type HistoricalPositionsAction =
  | { type: "LOAD_HISTORICAL_POSITIONS"; positions: HistoricalPosition[] }
  | {
      type: "LOAD_HISTORICAL_POSITIONS_SUCCEEDED";
      positions: HistoricalPosition[];
    }
  | {
      type: "UPDATE_HISTORICAL_POSITIONS";
      positions: HistoricalPosition[];
    }
  | { type: "LOAD_HISTORICAL_POSITIONS_FAILED"; error: string };

const historicalPositionsReducer = (
  state: HistoricalPositionsState,
  action: HistoricalPositionsAction
): HistoricalPositionsState => {
  switch (action.type) {
    case "LOAD_HISTORICAL_POSITIONS":
      return { type: "HISTORICAL_POSITIONS_LOADING", positions: action.positions };
    case "UPDATE_HISTORICAL_POSITIONS":
    case "LOAD_HISTORICAL_POSITIONS_SUCCEEDED":
      return {
        type: "HISTORICAL_POSITIONS_LOADED",
        positions: action.positions,
      };
    case "LOAD_HISTORICAL_POSITIONS_FAILED":
      return { type: "HISTORICAL_POSITIONS_LOAD_FAILED", error: action.error };
    default:
      return { type: "HISTORICAL_POSITIONS_UNLOADED" };
  }
};

const useFetchHistoricalPositions = (
  teamId: number,
  seasons: Season[],
  range: HistoricalPositionRange
) => {
  const api = useApi();
  const [allFetchedPositions, setAllFetchedPositions] = useState<HistoricalPosition[]>([]);
  const [url, setUrl] = useState<string>("");
  const [state, dispatch] = useReducer<
    Reducer<HistoricalPositionsState, HistoricalPositionsAction>
  >(historicalPositionsReducer, {
    type: "HISTORICAL_POSITIONS_UNLOADED",
  });

  const getPositionsInRange = (positions: HistoricalPosition[]) =>
    positions.filter(
      (x) => x.seasonStartYear >= range.startYear && x.seasonStartYear <= range.endYear
    );

  const getInclusiveYears = (start: number, end: number) =>
    Array.from({ length: end - start + 1 }, (v, k) => k + start);

  const getUrl = (yearsToFetch: number[]) => {
    const seasonIds = seasons
      .filter((x) => yearsToFetch.includes(x.startYear))
      .map((y) => `&seasonIds=${y.id}`)
      .join("");

    return `${api}/api/v2/historical-positions?teamId=${teamId}${seasonIds}`;
  };

  useEffect(() => {
    const allYearsInRange = getInclusiveYears(range.startYear, range.endYear);
    setAllFetchedPositions([]);
    const newUrl = getUrl(allYearsInRange);

    setUrl(newUrl);
  }, [teamId]);

  useEffect(() => {
    const allYearsInRange = getInclusiveYears(range.startYear, range.endYear);
    const positionsAlreadyFetched = allFetchedPositions.filter((x) =>
      allYearsInRange.includes(x.seasonStartYear)
    );

    const yearsNotYetFetched = allYearsInRange.filter(
      (x) => !positionsAlreadyFetched.map((y) => y.seasonStartYear).includes(x)
    );

    if (yearsNotYetFetched.length === 0) {
      dispatch({
        type: "UPDATE_HISTORICAL_POSITIONS",
        positions: getPositionsInRange(positionsAlreadyFetched),
      });
    }

    const newUrl = getUrl(yearsNotYetFetched);

    setUrl(newUrl);
  }, [range]);

  useEffect(() => {
    const abortController = new AbortController();

    dispatch({
      type: "LOAD_HISTORICAL_POSITIONS",
      positions: getPositionsInRange(allFetchedPositions),
    });

    fetch(url, {
      signal: abortController.signal,
    })
      .then((response) => response.json())
      .then((response) => {
        if (response.error === null) {
          const allPositions = [...allFetchedPositions, ...response.result];
          setAllFetchedPositions(allPositions);
          dispatch({
            type: "LOAD_HISTORICAL_POSITIONS_SUCCEEDED",
            positions: getPositionsInRange(allPositions),
          });
        } else {
          throw new Error(response.error.message);
        }
      })
      .catch((error) => {
        if (!abortController.signal.aborted) {
          dispatch({ type: "LOAD_HISTORICAL_POSITIONS_FAILED", error });
        }
      });

    return () => {
      abortController.abort();
    };
  }, [url]);

  return { state };
};

export { useFetchHistoricalPositions };
