import { Dispatch } from "react";
import { useDispatch, useSelector } from "react-redux";
import { LeagueTableAction, LeagueTableState } from "../reducers/leagueTableReducer";
import { AppState } from "../reducers/appReducer";

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
  table: Row[] | null;
};

const useLeagueTable = () => {
  const leagueTableState = useSelector<AppState, LeagueTableState>((s) => s.leagueTableState);
  const dispatch = useDispatch<Dispatch<LeagueTableAction>>();

  const getLeagueTableForTeam = (club: string, seasonStartYear: number) => {
    fetch(
      `https://localhost:5001/api/League/GetCompletedLeagueForTeam?seasonStartYear=${seasonStartYear}&team=${club}`
    )
      .then((response) => response.json())
      .then((response) => dispatch({ type: "LEAGUE_TABLE_LOAD_COMPLETED", leagueTable: response }))
      .catch(console.log);
  };

  const getLeagueTable = (tier: number, seasonStartYear: number) => {
    fetch(
      `https://localhost:5001/api/League/GetCompletedLeague?seasonStartYear=${seasonStartYear}&tier=${tier}`
    )
      .then((response) => response.json())
      .then((response) => dispatch({ type: "LEAGUE_TABLE_LOAD_COMPLETED", leagueTable: response }))
      .catch(console.log);
  };

  const clearLeagueTable = () => {
    dispatch({ type: "CLEAR_LEAGUE_TABLE" });
  };

  return { leagueTableState, getLeagueTable, getLeagueTableForTeam, clearLeagueTable };
};

export { useLeagueTable };
