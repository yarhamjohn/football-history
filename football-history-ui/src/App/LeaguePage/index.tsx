import React, { FunctionComponent, useEffect, useState } from "react";
import { Divider } from "semantic-ui-react";
import { CompetitionFilter } from "../components/Filters/CompetitionFilter";
import { AppSubPage } from "../App";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { Matches } from "./Matches";
import { Competition } from "../shared/useFetchCompetitions";
import { League } from "../components/League";
import { useAppDispatch, useAppSelector } from "../../hook";
import { selectSeason } from "../../seasonsSlice";

const LeaguePage: FunctionComponent<{
  activeSubPage: AppSubPage;
  setActiveSubPage: (subPage: AppSubPage) => void;
}> = ({ activeSubPage, setActiveSubPage }) => {
  const dispatch = useAppDispatch();
  const seasonState = useAppSelector((state) => state.season);

  const [selectedCompetition, setSelectedCompetition] =
    useState<Competition | undefined>(undefined);

  useEffect(() => {
    const season = seasonState.seasons.reduce(function (prev, current) {
      return prev.startYear > current.startYear ? prev : current;
    });

    dispatch(selectSeason(season));
  }, [seasonState.seasons, dispatch, selectSeason]);

  let body;
  if (activeSubPage === "Table") {
    body = selectedCompetition && (
      <>
        <SeasonFilter />
        {seasonState.selectedSeason && (
          <League
            props={{
              season: seasonState.selectedSeason,
              competition: selectedCompetition,
            }}
          />
        )}
      </>
    );
  } else if (activeSubPage === "Results") {
    body = selectedCompetition && (
      <div style={{ display: "grid", gridGap: "1rem" }}>
        <SeasonFilter />
        {seasonState.selectedSeason ? <Matches competitionId={selectedCompetition.id} /> : null}
      </div>
    );
  }

  return (
    <>
      {seasonState.selectedSeason && (
        <CompetitionFilter
          selectedSeason={seasonState.selectedSeason}
          selectedCompetition={selectedCompetition}
          selectCompetition={(name) => {
            setActiveSubPage(name ? "Table" : "None");
            setSelectedCompetition(name);
          }}
        />
      )}
      <Divider />
      {body}
    </>
  );
};

export { LeaguePage };
