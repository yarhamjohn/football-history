import React, { FunctionComponent, useEffect, useState } from "react";
import { Divider } from "semantic-ui-react";
import { Season, useSeasons } from "../hooks/useSeasons";
import { DivisionFilter } from "../components/Filters/LeagueFilter";
import { LeagueSeason } from "./LeagueSeason";
import { useLeagueTable } from "../hooks/useLeagueTable";

const LeaguePage: FunctionComponent = () => {
  const { seasonsState } = useSeasons();
  const { clearLeagueTable } = useLeagueTable();
  const [selectedDivision, setSelectedDivision] = useState<string | undefined>(undefined);

  useEffect(() => {
    clearLeagueTable();
  }, []);

  const getDivisionTier = (seasons: Season[], divisionName: string) => {
    const divisions = getDivisions(seasons).filter((d) => d.name === divisionName);

    if (divisions.length !== 1) {
      throw new Error(`The division name (${divisionName}) provided matches more than one tier.`);
    }

    return divisions[0].tier;
  };

  const getDivisions = (seasons: Season[]) => {
    const tiers = Array.from(
      new Set(
        seasons
          .map((s) => s.divisions)
          .flat()
          .map((d) => d.tier)
      )
    );

    let divisions = [];
    for (let tier of tiers) {
      const divs = Array.from(
        new Set(
          seasons
            .map((s) => s.divisions)
            .flat()
            .filter((d) => d.tier === tier)
            .map((d) => d.name)
        )
      );
      divisions.push({ name: divs.join(", "), tier: tier });
    }

    return divisions;
  };

  if (seasonsState.type !== "SEASONS_LOADED") {
    return null;
  }

  return (
    <>
      <DivisionFilter
        divisions={getDivisions(seasonsState.seasons)}
        selectedDivision={selectedDivision}
        selectDivision={(name) => setSelectedDivision(name)}
      />
      <Divider />
      {selectedDivision && (
        <LeagueSeason selectedTier={getDivisionTier(seasonsState.seasons, selectedDivision)} />
      )}
    </>
  );
};

export { LeaguePage };
