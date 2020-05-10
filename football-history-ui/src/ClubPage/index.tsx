import React, { FunctionComponent, useEffect, useState } from "react";
import { Divider } from "semantic-ui-react";
import { useClubs } from "../hooks/useClubs";
import { ClubFilter } from "../components/Filters/ClubFilter";
import { ClubSeason } from "./ClubSeason";
import { useLeagueTable } from "../hooks/useLeagueTable";

const ClubPage: FunctionComponent = () => {
  const { clubsState } = useClubs();
  const [selectedClub, setSelectedClub] = useState<string | undefined>(undefined);
  const { clearLeagueTable } = useLeagueTable();

  useEffect(() => {
    clearLeagueTable();
  }, []);

  if (clubsState.type !== "CLUBS_LOADED") {
    return null;
  }

  return (
    <>
      <ClubFilter
        clubs={clubsState.clubs}
        selectedClub={selectedClub}
        selectClub={(name) => setSelectedClub(name)}
      />
      <Divider />
      {selectedClub && <ClubSeason selectedClub={selectedClub} />}
    </>
  );
};

export { ClubPage };
