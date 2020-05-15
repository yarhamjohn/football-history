import { Dispatch } from "react";
import { useDispatch, useSelector } from "react-redux";
import { AppState } from "../reducers/appReducer";
import { SeasonsAction, SeasonsState } from "../reducers/seasonsReducer";

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

  const getDivisions = () => {
    if (seasonsState.type !== "SEASONS_LOADED") {
      return [];
    }

    const tiers = Array.from(
      new Set(
        seasonsState.seasons
          .map((s) => s.divisions)
          .flat()
          .map((d) => d.tier)
      )
    );

    let divisions = [];
    for (let tier of tiers) {
      const divs = Array.from(
        new Set(
          seasonsState.seasons
            .map((s) => s.divisions)
            .flat()
            .filter((d) => d.tier === tier)
            .map((d) => d.name)
        )
      );
      divisions.push({ name: divs.join(", "), tier: tier });
    }

    return divisions;
  };

  return { seasonsState, getSeasons, getDivisions };
};

export { useSeasons };
