import { AppAction } from "./appReducer";
import { Club } from "../hooks/useClubs";

export type ClubsState =
  | {
      type: "NO_CLUBS_LOADED";
    }
  | {
      type: "LOADING_CLUBS";
    }
  | {
      type: "LOADING_CLUBS_FAILED";
      error: string;
    }
  | {
      type: "CLUBS_LOADED";
      clubs: Club[];
    };

export type ClubsAction =
  | {
      type: "CLUBS_LOAD_STARTED";
    }
  | {
      type: "CLUBS_LOAD_COMPLETED";
      clubs: Club[];
    }
  | {
      type: "CLUBS_LOAD_FAILED";
      error: string;
    };

const clubsReducer = (prevState: ClubsState | undefined, action: AppAction): ClubsState => {
  if (!prevState) {
    return { type: "NO_CLUBS_LOADED" };
  }

  switch (action.type) {
    case "CLUBS_LOAD_STARTED":
      return { type: "LOADING_CLUBS" };
    case "CLUBS_LOAD_COMPLETED":
      return { type: "CLUBS_LOADED", clubs: action.clubs };
    case "CLUBS_LOAD_FAILED":
      return { type: "LOADING_CLUBS_FAILED", error: action.error };
  }

  return prevState;
};

export { clubsReducer };
