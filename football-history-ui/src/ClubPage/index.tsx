import React, { FunctionComponent, useEffect, useState } from "react";
import { Divider } from "semantic-ui-react";
import { useClubs } from "../hooks/useClubs";
import { ClubFilter } from "../components/Filters/ClubFilter";
import { Season } from "../hooks/useSeasons";
import { HistoricalPositions } from "../components/HistoricalPositions";
import { AppSubPage } from "../App";
import { LeagueTable } from "../components/LeagueTable/LeagueTable";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { Matches } from "./Matches";

const ClubPage: FunctionComponent<{
  seasons: Season[];
  activeSubPage: AppSubPage;
  setActiveSubPage: (subPage: AppSubPage) => void;
}> = ({ seasons, activeSubPage, setActiveSubPage }) => {
  const { clubState } = useClubs();
  const [selectedClub, setSelectedClub] = useState<string | undefined>(undefined);
  const [selectedSeason, setSelectedSeason] = useState<number | undefined>(undefined);

  useEffect(() => {
    setSelectedSeason(Math.max(...seasons.map((s) => s.startYear)));
  }, [seasons]);

  if (clubState.type !== "CLUBS_LOAD_SUCCEEDED" || seasons.length === 0) {
    return null;
  }

  let body;
  if (activeSubPage === "Positions") {
    body = selectedClub && <HistoricalPositions selectedClub={selectedClub} seasons={seasons} />;
  } else if (activeSubPage === "Table") {
    body = selectedClub && (
      <>
        <SeasonFilter
          seasons={seasons}
          selectedSeason={selectedSeason}
          selectSeason={(startYear) => setSelectedSeason(startYear)}
        />
        {selectedSeason && (
          <LeagueTable selectedSeason={selectedSeason} selectedClub={selectedClub} />
        )}
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
        clubs={clubState.clubs}
        selectedClub={selectedClub}
        selectClub={(name) => {
          setActiveSubPage(name ? "Positions" : "None");
          setSelectedClub(name);
        }}
      />
      <Divider />
      {body}
    </>
  );
};

export { ClubPage };
