import React, { FunctionComponent, useState } from "react";
import { Divider } from "semantic-ui-react";
import { Team, useFetchClubs } from "../shared/useFetchClubs";
import { ClubFilter } from "../components/Filters/ClubFilter";
import { AppSubPage } from "../App";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { Matches } from "./Matches";
import { HistoricalPositions } from "../components/HistoricalPositions";
import { League } from "../components/League";
import { useAppSelector } from "../../reduxHooks";

const ClubPage: FunctionComponent<{
  activeSubPage: AppSubPage;
  setActiveSubPage: (subPage: AppSubPage) => void;
}> = ({ activeSubPage, setActiveSubPage }) => {
  const seasonState = useAppSelector((state) => state.season);

  const clubs = useFetchClubs();
  const [selectedClub, setSelectedClub] = useState<Team | undefined>(undefined);

  if (clubs.status !== "LOAD_SUCCESSFUL" || seasonState.seasons.length === 0) {
    return null;
  }

  let body;
  if (activeSubPage === "Positions") {
    body = selectedClub && (
      <HistoricalPositions teamId={selectedClub.id} seasons={seasonState.seasons} />
    );
  } else if (activeSubPage === "Table") {
    body = selectedClub && (
      <>
        <SeasonFilter />
        {seasonState.selectedSeason && (
          <League props={{ season: seasonState.selectedSeason, team: selectedClub }} />
        )}
      </>
    );
  } else if (activeSubPage === "Results") {
    body = selectedClub && (
      <>
        <SeasonFilter />
        {seasonState.selectedSeason && (
          <Matches selectedSeason={seasonState.selectedSeason} selectedClub={selectedClub} />
        )}
      </>
    );
  }

  return (
    <>
      <ClubFilter
        clubs={clubs.data}
        selectedClub={selectedClub}
        selectClub={(club) => {
          setActiveSubPage(club ? "Positions" : "None");
          setSelectedClub(club);
        }}
      />
      <Divider />
      {body}
    </>
  );
};

export { ClubPage };
