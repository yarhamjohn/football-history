import React, { FunctionComponent } from "react";
import { LeagueTable } from "../components/LeagueTable/LeagueTable";
import { useLeague } from "../hooks/useLeagueTable";

const ClubSeason: FunctionComponent<{ selectedClub: string; selectedSeason: number }> = ({
  selectedClub,
  selectedSeason,
}) => {
  const { leagueState } = useLeague(selectedSeason, selectedClub);

  return (
    <div
      style={{
        display: "grid",
        gridTemplateColumns: "repeat(auto-fit, minmax(1100px, 1fr))",
        gridGap: "1rem",
      }}
    >
      <LeagueTable
        leagueState={leagueState}
        seasonStartYear={selectedSeason}
        selectedClub={selectedClub}
      />
    </div>
  );
};

export { ClubSeason };
