import React, { FunctionComponent, useEffect, useState } from "react";
import { Divider } from "semantic-ui-react";
import { Club, useClubs } from "../hooks/useClubs";
import { ClubFilter } from "../components/Filters/ClubFilter";
import { ClubSeason } from "./ClubSeason";
import { useLeagueTable } from "../hooks/useLeagueTable";

const ClubPage: FunctionComponent = () => {
  const [selectedClub, setSelectedClub] = useState<Club | undefined>(undefined);
  const { clearLeagueTable } = useLeagueTable();

  useEffect(() => {
    clearLeagueTable();
  }, []);

  return (
    <>
      <ClubFilter
        selectedClub={selectedClub}
        setSelectedClub={(selection: Club | undefined) => setSelectedClub(selection)}
      />
      <Divider />
      {selectedClub && <ClubSeason selectedClub={selectedClub} />}
    </>
  );
};

export { ClubPage };
