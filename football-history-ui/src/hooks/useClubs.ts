import { Dispatch } from "react";
import { useDispatch, useSelector } from "react-redux";
import { AppState } from "../reducers/appReducer";
import { ClubsAction, ClubsState } from "../reducers/clubsReducer";

export interface Club {
  name: string;
  abbreviation: string;
}

const useClubs = () => {
  const clubsState = useSelector<AppState, ClubsState>((s) => s.clubsState);
  const dispatch = useDispatch<Dispatch<ClubsAction>>();

  const getClubs = () => {
    dispatch({ type: "CLUBS_LOAD_STARTED" });

    fetch(`https://localhost:5001/api/Team/GetAllTeams`)
      .then((response) => response.json())
      .then((response) => dispatch({ type: "CLUBS_LOAD_COMPLETED", clubs: response }))
      .catch((error) => {
        dispatch({ type: "CLUBS_LOAD_FAILED", error });
      });
  };

  return { clubsState, getClubs };
};

export { useClubs };
