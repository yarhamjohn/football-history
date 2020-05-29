import { Reducer, useEffect, useReducer } from "react";
import { useApi } from "./useApi";

export interface PlayOffMatch {
  tier: number;
  round: "Semi-Final" | "Final";
  division: string;
  date: Date;
  homeTeam: string;
  homeTeamAbbreviation: string;
  awayTeam: string;
  awayTeamAbbreviation: string;
  homeGoals: number;
  awayGoals: number;
  extraTime: boolean;
  homeGoalsExtraTime: number;
  awayGoalsExtraTime: number;
  penaltyShootout: boolean;
  homePenaltiesTaken: number;
  homePenaltiesScored: number;
  awayPenaltiesTaken: number;
  awayPenaltiesScored: number;
}

export type PlayOffMatchesState =
  | { type: "PLAY_OFF_MATCHES_UNLOADED" }
  | { type: "PLAY_OFF_MATCHES_LOADING" }
  | { type: "PLAY_OFF_MATCHES_LOAD_SUCCEEDED"; matches: PlayOffMatch[] }
  | { type: "PLAY_OFF_MATCHES_LOAD_FAILED"; error: string };

type PlayOffMatchesAction =
  | { type: "LOAD_PLAY_OFF_MATCHES" }
  | { type: "LOAD_PLAY_OFF_MATCHES_SUCCEEDED"; matches: PlayOffMatch[] }
  | { type: "LOAD_PLAY_OFF_MATCHES_FAILED"; error: string };

const playOffMatchesReducer = (
  state: PlayOffMatchesState,
  action: PlayOffMatchesAction
): PlayOffMatchesState => {
  switch (action.type) {
    case "LOAD_PLAY_OFF_MATCHES":
      return { type: "PLAY_OFF_MATCHES_LOADING" };
    case "LOAD_PLAY_OFF_MATCHES_SUCCEEDED":
      return { type: "PLAY_OFF_MATCHES_LOAD_SUCCEEDED", matches: action.matches };
    case "LOAD_PLAY_OFF_MATCHES_FAILED":
      return { type: "PLAY_OFF_MATCHES_LOAD_FAILED", error: action.error };
    default:
      return { type: "PLAY_OFF_MATCHES_UNLOADED" };
  }
};

const usePlayOffMatches = (tier: number, seasonStartYear: number) => {
  const { api } = useApi();
  const [playOffMatchesState, dispatch] = useReducer<
    Reducer<PlayOffMatchesState, PlayOffMatchesAction>
  >(playOffMatchesReducer, {
    type: "PLAY_OFF_MATCHES_UNLOADED",
  });

  useEffect(() => {
    fetch(`${api}/api/Match/getPlayOffMatches?seasonStartYears=${seasonStartYear}&tiers=${tier}`)
      .then((response) => response.json())
      .then((response) => dispatch({ type: "LOAD_PLAY_OFF_MATCHES_SUCCEEDED", matches: response }))
      .catch((error) => {
        dispatch({ type: "LOAD_PLAY_OFF_MATCHES_FAILED", error });
      });
  }, [tier, seasonStartYear]);

  return { playOffMatchesState };
};

export { usePlayOffMatches };
