import { useState } from "react";

export type LeaguePosition = {
  date: string;
  position: number;
};

const useLeaguePositions = () => {
  const [leaguePositions, setLeaguePositions] = useState<LeaguePosition[]>();

  const getLeaguePositions = (club: string, seasonStartYear: number) => {
    fetch(
      `https://localhost:5001/api/Position/GetLeaguePositions?seasonStartYear=${seasonStartYear}&team=${club}`
    )
      .then((response) => response.json())
      .then((response) => setLeaguePositions(response))
      .catch(console.log);
  };

  return { leaguePositions, getLeaguePositions };
};

export { useLeaguePositions };
