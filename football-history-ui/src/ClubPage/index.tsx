import React, { FunctionComponent, useState } from "react";
import { Divider } from "semantic-ui-react";
import { useClubs } from "../hooks/useClubs";
import { ClubFilter } from "../components/Filters/ClubFilter";
import { Season } from "../components/Season";
import { Season as SeasonType } from "../hooks/useSeasons";

const ClubPage: FunctionComponent<{ seasons: SeasonType[] }> = ({ seasons }) => {
  const { clubState } = useClubs();
  const [selectedClub, setSelectedClub] = useState<string | undefined>(undefined);

  if (clubState.type !== "CLUBS_LOAD_SUCCEEDED" || seasons.length === 0) {
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
      {selectedClub && <Season selectedClub={selectedClub} seasons={seasons} />}
    </>
  );
};

export { ClubPage };
