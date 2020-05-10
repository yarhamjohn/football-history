import React, { FunctionComponent, useEffect, useState } from "react";
import { Divider } from "semantic-ui-react";
import { Season, useSeasons } from "../hooks/useSeasons";
import { DivisionFilter } from "../components/Filters/LeagueFilter";
import { LeagueSeason } from "./LeagueSeason";
import { useLeagueTable } from "../hooks/useLeagueTable";

const LeaguePage: FunctionComponent = () => {
  const { seasonsState, getDivisions } = useSeasons();
  const { clearLeagueTable } = useLeagueTable();
  const [selectedDivision, setSelectedDivision] = useState<string | undefined>(undefined);

  useEffect(() => {
    clearLeagueTable();
  }, []);

  const getDivisionTier = (divisionName: string) => {
    const divisions = getDivisions().filter((d) => d.name === divisionName);

    if (divisions.length !== 1) {
      throw new Error(`The division name (${divisionName}) provided matches more than one tier.`);
    }

    return divisions[0].tier;
  };

  if (seasonsState.type !== "SEASONS_LOADED") {
    return null;
  }

  return (
    <>
      <DivisionFilter
        divisions={getDivisions()}
        selectedDivision={selectedDivision}
        selectDivision={(name) => setSelectedDivision(name)}
      />
      <Divider />
      {selectedDivision && <LeagueSeason selectedTier={getDivisionTier(selectedDivision)} />}
    </>
  );
};

export { LeaguePage };
