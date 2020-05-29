import React, { FunctionComponent, useState } from "react";
import { Divider } from "semantic-ui-react";
import { useClubs } from "../hooks/useClubs";
import { ClubFilter } from "../components/Filters/ClubFilter";
import { Season } from "../components/Season";
import { Season as SeasonType } from "../hooks/useSeasons";
import { HistoricalPositions } from "../components/HistoricalPositions";
import { AppSubPage } from "../App";

const ClubPage: FunctionComponent<{
  seasons: SeasonType[];
  activeSubPage: AppSubPage;
  setActiveSubPage: (subPage: AppSubPage) => void;
}> = ({ seasons, activeSubPage, setActiveSubPage }) => {
  const { clubState } = useClubs();
  const [selectedClub, setSelectedClub] = useState<string | undefined>(undefined);

  if (clubState.type !== "CLUBS_LOAD_SUCCEEDED" || seasons.length === 0) {
    return null;
  }

  let body;
  if (activeSubPage === "Positions") {
    body = selectedClub && <HistoricalPositions selectedClub={selectedClub} seasons={seasons} />;
  } else if (activeSubPage === "Table") {
    body = selectedClub && <Season selectedClub={selectedClub} seasons={seasons} />;
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
