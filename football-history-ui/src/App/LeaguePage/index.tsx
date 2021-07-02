import React, { FunctionComponent } from "react";
import { Divider } from "semantic-ui-react";
import { CompetitionFilter } from "../components/Filters/CompetitionFilter";
import { AppSubPage } from "../App";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { Matches } from "./Matches";
import { League } from "../components/League";
import { useAppSelector } from "../../reduxHooks";

const LeaguePage: FunctionComponent<{
  activeSubPage: AppSubPage;
  setActiveSubPage: (subPage: AppSubPage) => void;
}> = ({ activeSubPage, setActiveSubPage }) => {
  const seasonState = useAppSelector((state) => state.season);
  const competitionState = useAppSelector((state) => state.competition);

  let body;
  if (activeSubPage === "Table") {
    body = competitionState.selectedCompetition && (
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
    );
  } else if (activeSubPage === "Results") {
    body = competitionState.selectedCompetition && (
      <div style={{ display: "grid", gridGap: "1rem" }}>
        <SeasonFilter />
        {seasonState.selectedSeason ? (
          <Matches competitionId={competitionState.selectedCompetition.id} />
        ) : null}
      </div>
    );
  }

  return (
    <>
      {seasonState.selectedSeason && <CompetitionFilter setActiveSubPage={setActiveSubPage} />}
      <Divider />
      {body}
    </>
  );
};

export { LeaguePage };
