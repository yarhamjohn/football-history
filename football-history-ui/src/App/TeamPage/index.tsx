import React, { FunctionComponent } from "react";
import { Divider } from "semantic-ui-react";
import { TeamFilter } from "../components/Filters/TeamFilter";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { Matches } from "./Matches";
import { HistoricalPositions } from "../components/HistoricalPositions";
import { League } from "../components/League";
import { useAppSelector } from "../../reduxHooks";
import { Team } from "../shared/teamsSlice";

const TeamPage: FunctionComponent = () => {
  const seasonState = useAppSelector((state) => state.season);
  const teamState = useAppSelector((state) => state.team);

  return (
    <>
      <TeamFilter />
      <Divider />
      {teamState.selectedTeamId === undefined ? null : (
        <>
          <h2>League positions by season</h2>
          <HistoricalPositions teamId={teamState.selectedTeamId} seasons={seasonState.seasons} />
          {seasonState.selectedSeason && (
            <>
              <Divider />
              <div
                style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}
              >
                <h2>League table for season:</h2>
                <SeasonFilter />
              </div>
              <League props={{ season: seasonState.selectedSeason, team: {} as Team }} />
              <h2>League matches</h2>
              <Matches
                selectedSeason={seasonState.selectedSeason}
                selectedTeam={teamState.selectedTeam}
              />
            </>
          )}
        </>
      )}
    </>
  );
};

export { TeamPage };
