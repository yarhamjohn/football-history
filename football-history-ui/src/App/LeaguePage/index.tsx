import React, { FunctionComponent } from "react";
import { Divider } from "semantic-ui-react";
import { CompetitionFilter } from "../components/Filters/CompetitionFilter";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { Matches } from "./Matches";
import { League } from "../components/League";
import { useAppSelector } from "../../reduxHooks";

const LeaguePage: FunctionComponent = () => {
  const seasonState = useAppSelector((state) => state.season);
  const competitionState = useAppSelector((state) => state.competition);

  return (
    <>
      {seasonState.selectedSeason && <CompetitionFilter />}
      <Divider />
      {competitionState.selectedCompetition && (
        <>
          <SeasonFilter />
          {seasonState.selectedSeason && (
            <League
              props={{
                season: seasonState.selectedSeason,
                competition: competitionState.selectedCompetition,
              }}
            />
          )}
        </>
      )}
      {competitionState.selectedCompetition && (
        <div style={{ display: "grid", gridGap: "1rem" }}>
          {seasonState.selectedSeason ? (
            <Matches competitionId={competitionState.selectedCompetition.id} />
          ) : null}
        </div>
      )}
    </>
  );
};

export { LeaguePage };
