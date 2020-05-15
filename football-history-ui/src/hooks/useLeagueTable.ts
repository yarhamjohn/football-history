import { Reducer, useEffect, useReducer } from "react";

export type Row = {
  position: number;
  team: string;
  played: number;
  won: number;
  drawn: number;
  lost: number;
  goalsFor: number;
  goalsAgainst: number;
  goalDifference: number;
  points: number;
  pointsDeducted: number;
  pointsDeductionReason: string | null;
  status: string | null;
};

export type League = {
  name: string | null;
  tier: number;
  totalPlaces: number;
  promotionPlaces: number;
  playOffPlaces: number;
  relegationPlaces: number;
  pointsForWin: number;
  startYear: number;
  table: Row[] | null; // TODO: This shouldn't be how its done
};

export type LeagueState =
  | { type: "LEAGUE_UNLOADED" }
  | { type: "LEAGUE_LOADING" }
  | { type: "LEAGUE_LOAD_SUCCEEDED"; league: League }
  | { type: "LEAGUE_LOAD_FAILED"; error: string };

type LeagueAction =
  | { type: "LOAD_LEAGUE" }
  | { type: "LOAD_LEAGUE_SUCCEEDED"; league: League }
  | { type: "LOAD_LEAGUE_FAILED"; error: string };

const leagueReducer = (state: LeagueState, action: LeagueAction): LeagueState => {
  switch (action.type) {
    case "LOAD_LEAGUE":
      return { type: "LEAGUE_LOADING" };
    case "LOAD_LEAGUE_SUCCEEDED":
      return { type: "LEAGUE_LOAD_SUCCEEDED", league: action.league };
    case "LOAD_LEAGUE_FAILED":
      return { type: "LEAGUE_LOAD_FAILED", error: action.error };
    default:
      return { type: "LEAGUE_UNLOADED" };
  }
};

const useLeague = (seasonStartYear: number, club?: string, tier?: number) => {
  const [leagueState, dispatch] = useReducer<Reducer<LeagueState, LeagueAction>>(leagueReducer, {
    type: "LEAGUE_UNLOADED",
  });

  useEffect(() => {
    if (!!club && !!tier) {
      throw new Error(
        `Neither a tier nor a club was specified when requesting the league table for season starting: ${seasonStartYear}`
      );
    }

    dispatch({ type: "LOAD_LEAGUE" });

    let url = club
      ? `https://localhost:5001/api/League/GetCompletedLeagueForTeam?seasonStartYear=${seasonStartYear}&team=${club}`
      : `https://localhost:5001/api/League/GetCompletedLeague?seasonStartYear=${seasonStartYear}&tier=${tier}`;

    fetch(url)
      .then((response) => response.json())
      .then((response) => dispatch({ type: "LOAD_LEAGUE_SUCCEEDED", league: response }))
      .catch((error) => {
        dispatch({ type: "LOAD_LEAGUE_FAILED", error });
      });
  }, [seasonStartYear, club, tier]);

  return { leagueState };
};

export { useLeague };
