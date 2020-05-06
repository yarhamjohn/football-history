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
  const [allLeagueMatches, setAllLeagueMatches] = useState<LeagueMatch[]>();

  const getLeagueMatches = (club: string, seasonStartYear: number) => {
    fetch(
      `https://localhost:5001/api/Match/GetLeagueMatches?seasonStartYears=${seasonStartYear}&teams=${club}`
    )
      .then((response) => response.json())
      .then((response) => setLeagueMatches(response))
      .catch(console.log);
  };

  const getAllLeagueMatches = (tier: number, seasonStartYear: number) => {
    fetch(
      `https://localhost:5001/api/Match/GetLeagueMatches?seasonStartYears=${seasonStartYear}&tiers=${tier}`
    )
      .then((response) => response.json())
      .then((response) => setAllLeagueMatches(response))
      .catch(console.log);
  };

  return { leagueMatches, allLeagueMatches, getLeagueMatches, getAllLeagueMatches };
};

export { useLeagueMatches };
