import { AppAction } from "./appReducer";
import { Season } from "../hooks/useSeasons";

export type SeasonsState =
  | {
      type: "NO_SEASONS_LOADED";
    }
  | {
      type: "LOADING_SEASONS";
    }
  | {
      type: "LOADING_SEASONS_FAILED";
      error: string;
    }
  | {
      type: "SEASONS_LOADED";
      seasons: Season[];
    };

export type SeasonsAction =
  | {
      type: "SEASONS_LOAD_STARTED";
    }
  | {
      type: "SEASONS_LOAD_COMPLETED";
      seasons: Season[];
    }
  | {
      type: "SEASONS_LOAD_FAILED";
      error: string;
    };

const seasonsReducer = (prevState: SeasonsState | undefined, action: AppAction): SeasonsState => {
  if (!prevState) {
    return { type: "NO_SEASONS_LOADED" };
  }

  switch (action.type) {
    case "SEASONS_LOAD_STARTED":
      return { type: "LOADING_SEASONS" };
    case "SEASONS_LOAD_COMPLETED":
      return { type: "SEASONS_LOADED", seasons: action.seasons };
    case "SEASONS_LOAD_FAILED":
      return { type: "LOADING_SEASONS_FAILED", error: action.error };
  }

  return prevState;
};

export { seasonsReducer };
