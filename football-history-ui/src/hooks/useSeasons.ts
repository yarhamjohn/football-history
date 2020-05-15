import { Reducer, useEffect, useReducer } from "react";

export interface Division {
  name: string;
  tier: number;
}

export interface Season {
  startYear: number;
  endYear: number;
  divisions: Division[];
}

export type SeasonState =
  | { type: "SEASONS_UNLOADED" }
  | { type: "SEASONS_LOADING" }
  | { type: "SEASONS_LOAD_SUCCEEDED"; seasons: Season[] }
  | { type: "SEASONS_LOAD_FAILED"; error: string };

type SeasonAction =
  | { type: "LOAD_SEASONS" }
  | { type: "LOAD_SEASONS_SUCCEEDED"; seasons: Season[] }
  | { type: "LOAD_SEASONS_FAILED"; error: string };

const seasonsReducer = (state: SeasonState, action: SeasonAction): SeasonState => {
  switch (action.type) {
    case "LOAD_SEASONS":
      return { type: "SEASONS_LOADING" };
    case "LOAD_SEASONS_SUCCEEDED":
      return { type: "SEASONS_LOAD_SUCCEEDED", seasons: action.seasons };
    case "LOAD_SEASONS_FAILED":
      return { type: "SEASONS_LOAD_FAILED", error: action.error };
    default:
      return { type: "SEASONS_UNLOADED" };
  }
};

const useSeasons = () => {
  const [seasonState, dispatch] = useReducer<Reducer<SeasonState, SeasonAction>>(seasonsReducer, {
    type: "SEASONS_UNLOADED",
  });

  useEffect(() => {
    dispatch({ type: "LOAD_SEASONS" });

    fetch(`https://localhost:5001/api/Season/GetSeasons`)
      .then((response) => response.json())
      .then((response) => dispatch({ type: "LOAD_SEASONS_SUCCEEDED", seasons: response }))
      .catch((error) => {
        dispatch({ type: "LOAD_SEASONS_FAILED", error });
      });
  }, []);

  return { seasonState };
};

export { useSeasons };
