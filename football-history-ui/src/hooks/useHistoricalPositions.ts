import { Reducer, useEffect, useReducer } from "react";

export type HistoricalPosition = {
  seasonStartYear: number;
  tier: number;
  position: number;
  absolutePosition: number;
  status: string;
};

export type HistoricalPositionsState =
  | { type: "HISTORICAL_POSITIONS_UNLOADED" }
  | { type: "HISTORICAL_POSITIONS_LOADING" }
  | { type: "HISTORICAL_POSITIONS_LOAD_SUCCEEDED"; positions: HistoricalPosition[] }
  | { type: "HISTORICAL_POSITIONS_LOAD_FAILED"; error: string };

type HistoricalPositionsAction =
  | { type: "LOAD_HISTORICAL_POSITIONS" }
  | { type: "LOAD_HISTORICAL_POSITIONS_SUCCEEDED"; positions: HistoricalPosition[] }
  | { type: "LOAD_HISTORICAL_POSITIONS_FAILED"; error: string };

const historicalPositionsReducer = (
  state: HistoricalPositionsState,
  action: HistoricalPositionsAction
): HistoricalPositionsState => {
  switch (action.type) {
    case "LOAD_HISTORICAL_POSITIONS":
      return { type: "HISTORICAL_POSITIONS_LOADING" };
    case "LOAD_HISTORICAL_POSITIONS_SUCCEEDED":
      return { type: "HISTORICAL_POSITIONS_LOAD_SUCCEEDED", positions: action.positions };
    case "LOAD_HISTORICAL_POSITIONS_FAILED":
      return { type: "HISTORICAL_POSITIONS_LOAD_FAILED", error: action.error };
    default:
      return { type: "HISTORICAL_POSITIONS_UNLOADED" };
  }
};

const useHistoricalPositions = (
  club: string,
  firstSeasonStartYear: number,
  lastSeasonEndYear: number
) => {
  const [historicalPositionsState, dispatch] = useReducer<
    Reducer<HistoricalPositionsState, HistoricalPositionsAction>
  >(historicalPositionsReducer, {
    type: "HISTORICAL_POSITIONS_UNLOADED",
  });

  useEffect(() => {
    fetch(
      `https://localhost:5001/api/Position/GetHistoricalPositions?startYear=${firstSeasonStartYear}&endYear=${lastSeasonEndYear}&team=${club}`
    )
      .then((response) => response.json())
      .then((response) =>
        dispatch({ type: "LOAD_HISTORICAL_POSITIONS_SUCCEEDED", positions: response })
      )
      .catch((error) => {
        dispatch({ type: "LOAD_HISTORICAL_POSITIONS_FAILED", error });
      });
  }, [club, firstSeasonStartYear, lastSeasonEndYear]);

  return { historicalPositionsState };
};

export { useHistoricalPositions };
