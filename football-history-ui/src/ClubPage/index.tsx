import React, { FunctionComponent, useState } from "react";
import { Divider } from "semantic-ui-react";
import { Club, useClubs } from "./useClubs";
import { ClubFilter } from "./ClubFilter";
import { Season } from "./Season";

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
      {selectedClub && <Season selectedClub={selectedClub} />}
    </>
  );
};

export { ClubPage };
