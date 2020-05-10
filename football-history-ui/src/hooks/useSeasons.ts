import { Dispatch } from "react";
import { useDispatch, useSelector } from "react-redux";
import { AppState } from "../reducers/appReducer";
import { SeasonsAction, SeasonsState } from "../reducers/SeasonsReducer";

export interface Division {
  name: string;
  tier: number;
}

export interface Season {
  startYear: number;
  endYear: number;
  divisions: Division[];
}

const useSeasons = () => {
  const seasonsState = useSelector<AppState, SeasonsState>((s) => s.seasonsState);
  const dispatch = useDispatch<Dispatch<SeasonsAction>>();

  const getSeasons = () => {
    dispatch({ type: "SEASONS_LOAD_STARTED" });

    fetch(`https://localhost:5001/api/Season/GetSeasons`)
      .then((response) => response.json())
      .then((response) => dispatch({ type: "SEASONS_LOAD_COMPLETED", seasons: response }))
      .catch((error) => {
        dispatch({ type: "SEASONS_LOAD_FAILED", error });
      });
  };

  return { seasonsState, getSeasons };
};

export { useSeasons };
