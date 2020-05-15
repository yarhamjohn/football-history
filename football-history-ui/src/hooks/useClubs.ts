import { Reducer, useEffect, useReducer } from "react";

export interface Club {
  name: string;
  abbreviation: string;
}

export type ClubState =
  | { type: "CLUBS_UNLOADED" }
  | { type: "CLUBS_LOADING" }
  | { type: "CLUBS_LOAD_SUCCEEDED"; clubs: Club[] }
  | { type: "CLUBS_LOAD_FAILED"; error: string };

type ClubAction =
  | { type: "LOAD_CLUBS" }
  | { type: "LOAD_CLUBS_SUCCEEDED"; clubs: Club[] }
  | { type: "LOAD_CLUBS_FAILED"; error: string };

const clubsReducer = (state: ClubState, action: ClubAction): ClubState => {
  switch (action.type) {
    case "LOAD_CLUBS":
      return { type: "CLUBS_LOADING" };
    case "LOAD_CLUBS_SUCCEEDED":
      return { type: "CLUBS_LOAD_SUCCEEDED", clubs: action.clubs };
    case "LOAD_CLUBS_FAILED":
      return { type: "CLUBS_LOAD_FAILED", error: action.error };
    default:
      return { type: "CLUBS_UNLOADED" };
  }
};

const useClubs = () => {
  const [clubState, dispatch] = useReducer<Reducer<ClubState, ClubAction>>(clubsReducer, {
    type: "CLUBS_UNLOADED",
  });

  useEffect(() => {
    dispatch({ type: "LOAD_CLUBS" });

    fetch(`https://localhost:5001/api/Team/GetAllTeams`)
      .then((response) => response.json())
      .then((response) => dispatch({ type: "LOAD_CLUBS_SUCCEEDED", clubs: response }))
      .catch((error) => {
        dispatch({ type: "LOAD_CLUBS_FAILED", error });
      });
  }, []);

  return { clubState };
};

export { useClubs };
