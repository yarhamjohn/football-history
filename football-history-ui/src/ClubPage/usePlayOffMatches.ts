import { useState } from "react";

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

const usePlayOffMatches = () => {
  const [playOffMatches, setPlayOffMatches] = useState<PlayOffMatch[]>([]);

  const getPlayOffMatches = (tier: number, seasonStartYear: number) => {
    fetch(
      `https://localhost:5001/api/Match/getPlayOffMatches?seasonStartYears=${seasonStartYear}&tiers=${tier}`
    )
      .then((response) => response.json())
      .then((response) => setPlayOffMatches(response))
      .catch(console.log);
  };

  return { playOffMatches, getPlayOffMatches };
};

export { usePlayOffMatches };
