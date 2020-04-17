import { useState } from "react";

export interface LeagueTableRow {
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
}

export interface LeagueTable {
  name: string | null;
  tier: number;
  totalPlaces: number;
  promotionPlaces: number;
  playOffPlaces: number;
  relegationPlaces: number;
  pointsForWin: number;
  startYear: number;
  table: LeagueTableRow[] | null;
}

const useLeagueTable = () => {
  const [leagueTable, setLeagueTable] = useState<LeagueTable>();

  const getLeagueTable = (club: string, seasonStartYear: number) => {
    fetch(
      `https://localhost:5001/api/League/GetCompletedLeagueForTeam?seasonStartYear=${seasonStartYear}&team=${club}`
    )
      .then((response) => response.json())
      .then((response) => setLeagueTable(response))
      .catch(console.log);
  };

  return { leagueTable, getLeagueTable };
};

export { useLeagueTable };
