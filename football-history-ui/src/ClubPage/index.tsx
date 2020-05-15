import React, { FunctionComponent, useState } from "react";
import { Divider } from "semantic-ui-react";
import { useClubs } from "../hooks/useClubs";
import { ClubFilter } from "../components/Filters/ClubFilter";
import { SeasonState } from "../hooks/useSeasons";
import { Season } from "../components/Season";

const ClubPage: FunctionComponent<{ seasonState: SeasonState }> = ({ seasonState }) => {
  const { clubState } = useClubs();
  const [selectedClub, setSelectedClub] = useState<string | undefined>(undefined);

  if (clubState.type !== "CLUBS_LOAD_SUCCEEDED") {
    return null;
  }

  return (
    <>
      <ClubFilter
        clubs={clubState.clubs}
        selectedClub={selectedClub}
        selectClub={(name) => setSelectedClub(name)}
      />
      <Divider />
      {selectedClub && <Season selectedClub={selectedClub} seasonState={seasonState} />}
    </>
  );
};

export { ClubPage };
