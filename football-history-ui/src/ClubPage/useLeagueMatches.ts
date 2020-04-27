import { useState } from "react";

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

const useLeagueMatches = () => {
  const [leagueMatches, setLeagueMatches] = useState<LeagueMatch[]>();

  const getLeagueMatches = (club: string, seasonStartYear: number) => {
    fetch(
      `https://localhost:5001/api/Match/GetLeagueMatches?seasonStartYears=${seasonStartYear}&teams=${club}`
    )
      .then((response) => response.json())
      .then((response) => setLeagueMatches(response))
      .catch(console.log);
  };

  return { leagueMatches, getLeagueMatches };
};

export { useLeagueMatches };
