import React, { FunctionComponent, useEffect, useState } from "react";
import { Divider } from "semantic-ui-react";
import { Team, useFetchClubs } from "../shared/useFetchClubs";
import { ClubFilter } from "../components/Filters/ClubFilter";
import { Season } from "../shared/useFetchSeasons";
import { AppSubPage } from "../App";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { Matches } from "./Matches";
import { HistoricalPositions } from "../components/HistoricalPositions";
import { League } from "../components/League";

const ClubPage: FunctionComponent<{
  seasons: Season[];
  activeSubPage: AppSubPage;
  setActiveSubPage: (subPage: AppSubPage) => void;
}> = ({ seasons, activeSubPage, setActiveSubPage }) => {
  const clubs = useFetchClubs();
  const [selectedClub, setSelectedClub] = useState<Team | undefined>(undefined);
  const [selectedSeason, setSelectedSeason] = useState<Season | undefined>(undefined);

  useEffect(() => {
    const season = seasons.reduce(function (prev, current) {
      return prev.startYear > current.startYear ? prev : current;
    });

    setSelectedSeason(season);
  }, [seasons]);

  if (clubs.status !== "LOAD_SUCCESSFUL" || seasons.length === 0) {
    return null;
  }

  let body;
  if (activeSubPage === "Positions") {
    body = selectedClub && <HistoricalPositions teamId={selectedClub.id} seasons={seasons} />;
  } else if (activeSubPage === "Table") {
    body = selectedClub && (
      <>
        <SeasonFilter
          seasons={seasons}
          selectedSeason={selectedSeason}
          selectSeason={(startYear) => setSelectedSeason(startYear)}
        />
        {selectedSeason && <League props={{ season: selectedSeason, team: selectedClub }} />}
      </>
    );
  } else if (activeSubPage === "Results") {
    body = selectedClub && (
      <>
        <SeasonFilter
          seasons={seasons}
          selectedSeason={selectedSeason}
          selectSeason={(startYear) => setSelectedSeason(startYear)}
        />
        {selectedSeason && <Matches selectedSeason={selectedSeason} selectedClub={selectedClub} />}
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
