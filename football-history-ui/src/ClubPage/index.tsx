import React, { FunctionComponent, useState } from "react";
import { Divider } from "semantic-ui-react";
import { Club, useClubs } from "../hooks/useClubs";
import { ClubFilter } from "../components/Filters/ClubFilter";
import { ClubSeason } from "./ClubSeason";

const ClubPage: FunctionComponent = () => {
  const { clubs } = useClubs();
  const [selectedClub, setSelectedClub] = useState<Club | undefined>(undefined);

  return (
    <>
      <ClubFilter
        clubs={clubs}
        selectedClub={selectedClub}
        setSelectedClub={(selection: Club | undefined) => setSelectedClub(selection)}
      />
      <Divider />
      {selectedClub && <ClubSeason selectedClub={selectedClub} />}
    </>
  );
};

export { ClubPage };
