import { Reducer, useEffect, useReducer } from "react";
import { useApi } from "./useApi";

export interface LeagueMatch {
    tier: number;
    division: string;
    date: Date;
    homeTeam: string;
    homeTeamAbbreviation: string;
    awayTeam: string;
    awayTeamAbbreviation: string;
    homeGoals: number;
    awayGoals: number;
}

export type LeagueMatchesState =
    | { type: "LEAGUE_MATCHES_UNLOADED" }
    | { type: "LEAGUE_MATCHES_LOADING" }
    | { type: "LEAGUE_MATCHES_LOAD_SUCCEEDED"; matches: LeagueMatch[] }
    | { type: "LEAGUE_MATCHES_LOAD_FAILED"; error: string };

type LeagueMatchesAction =
    | { type: "LOAD_LEAGUE_MATCHES" }
    | { type: "LOAD_LEAGUE_MATCHES_SUCCEEDED"; matches: LeagueMatch[] }
    | { type: "LOAD_LEAGUE_MATCHES_FAILED"; error: string };

const leagueMatchesReducer = (
    state: LeagueMatchesState,
    action: LeagueMatchesAction
): LeagueMatchesState => {
    switch (action.type) {
        case "LOAD_LEAGUE_MATCHES":
            return { type: "LEAGUE_MATCHES_LOADING" };
        case "LOAD_LEAGUE_MATCHES_SUCCEEDED":
            return { type: "LEAGUE_MATCHES_LOAD_SUCCEEDED", matches: action.matches };
        case "LOAD_LEAGUE_MATCHES_FAILED":
            return { type: "LEAGUE_MATCHES_LOAD_FAILED", error: action.error };
        default:
            return { type: "LEAGUE_MATCHES_UNLOADED" };
    }
};

const useLeagueMatches = (seasonStartYear: number, club?: string, tier?: number) => {
    const api = useApi();
    const [leagueMatchesState, dispatch] = useReducer<
        Reducer<LeagueMatchesState, LeagueMatchesAction>
    >(leagueMatchesReducer, {
        type: "LEAGUE_MATCHES_UNLOADED",
    });

    useEffect(() => {
        let url = club
            ? `${api}/api/v1/Match/GetLeagueMatches?seasonStartYears=${seasonStartYear}&teams=${club}`
            : `${api}/api/v1/Match/GetLeagueMatches?seasonStartYears=${seasonStartYear}&tiers=${tier}`;

        fetch(url)
            .then((response) => response.json())
            .then((response) =>
                dispatch({ type: "LOAD_LEAGUE_MATCHES_SUCCEEDED", matches: response })
            )
            .catch((error) => {
                dispatch({ type: "LOAD_LEAGUE_MATCHES_FAILED", error });
            });
    }, [seasonStartYear, club, tier]);

    return { leagueMatchesState };
};

export { useLeagueMatches };
