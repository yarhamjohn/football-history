import React, { FunctionComponent, useEffect, useState } from "react";
import { Divider } from "semantic-ui-react";
import { Division, useSeasons } from "../hooks/useSeasons";
import { DivisionFilter } from "../components/Filters/LeagueFilter";
import { LeagueSeason } from "./LeagueSeason";

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
      <LeagueSeason selectedTier={selectedDivision?.tier} />
    </>
  );
};

export { LeaguePage };
