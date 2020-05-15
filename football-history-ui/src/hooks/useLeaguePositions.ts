import { Reducer, useEffect, useReducer } from "react";

export type LeaguePosition = {
  date: string;
  position: number;
};

export type LeaguePositionsState =
  | { type: "LEAGUE_POSITIONS_UNLOADED" }
  | { type: "LEAGUE_POSITIONS_LOADING" }
  | { type: "LEAGUE_POSITIONS_LOAD_SUCCEEDED"; positions: LeaguePosition[] }
  | { type: "LEAGUE_POSITIONS_LOAD_FAILED"; error: string };

type LeaguePositionsAction =
  | { type: "LOAD_LEAGUE_POSITIONS" }
  | { type: "LOAD_LEAGUE_POSITIONS_SUCCEEDED"; positions: LeaguePosition[] }
  | { type: "LOAD_LEAGUE_POSITIONS_FAILED"; error: string };

const leaguePositionsReducer = (
  state: LeaguePositionsState,
  action: LeaguePositionsAction
): LeaguePositionsState => {
  switch (action.type) {
    case "LOAD_LEAGUE_POSITIONS":
      return { type: "LEAGUE_POSITIONS_LOADING" };
    case "LOAD_LEAGUE_POSITIONS_SUCCEEDED":
      return { type: "LEAGUE_POSITIONS_LOAD_SUCCEEDED", positions: action.positions };
    case "LOAD_LEAGUE_POSITIONS_FAILED":
      return { type: "LEAGUE_POSITIONS_LOAD_FAILED", error: action.error };
    default:
      return { type: "LEAGUE_POSITIONS_UNLOADED" };
  }
};

const useLeaguePositions = (club: string, seasonStartYear: number) => {
  const [leaguePositionsState, dispatch] = useReducer<
    Reducer<LeaguePositionsState, LeaguePositionsAction>
  >(leaguePositionsReducer, {
    type: "LEAGUE_POSITIONS_UNLOADED",
  });

  useEffect(() => {
    fetch(
      `https://localhost:5001/api/Position/GetLeaguePositions?seasonStartYear=${seasonStartYear}&team=${club}`
    )
      .then((response) => response.json())
      .then((response) =>
        dispatch({ type: "LOAD_LEAGUE_POSITIONS_SUCCEEDED", positions: response })
      )
      .catch((error) => {
        dispatch({ type: "LOAD_LEAGUE_POSITIONS_FAILED", error });
      });
  }, [club, seasonStartYear]);

  return { leaguePositionsState };
};

export { useLeaguePositions };
