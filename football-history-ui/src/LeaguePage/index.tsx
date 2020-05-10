import React, { FunctionComponent, useEffect, useState } from "react";
import { Divider } from "semantic-ui-react";
import { Division, useSeasons } from "../hooks/useSeasons";
import { DivisionFilter } from "../components/Filters/LeagueFilter";

const LeaguePage: FunctionComponent = () => {
  const { divisions } = useSeasons();
  const [selectedDivision, setSelectedDivision] = useState<Division | undefined>(undefined);

  return (
    <>
      <DivisionFilter
        divisions={divisions}
        selectedDivision={selectedDivision}
        setSelectedDivision={(selection: Division | undefined) => setSelectedDivision(selection)}
      />
      <Divider />
      {/*{selectedLeague && <Season selectedLeague={selectedLeague} />}*/}
    </>
  );
};

export { LeaguePage };
