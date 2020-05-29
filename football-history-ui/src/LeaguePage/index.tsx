import React, { FunctionComponent, useState } from "react";
import { Divider } from "semantic-ui-react";
import { Season as SeasonType } from "../hooks/useSeasons";
import { DivisionFilter } from "../components/Filters/LeagueFilter";
import { Season } from "../components/Season";
import { AppSubPage } from "../App";
import { HistoricalPositions } from "../components/HistoricalPositions";

const LeaguePage: FunctionComponent<{
  seasons: SeasonType[];
  activeSubPage: AppSubPage;
  setActiveSubPage: (subPage: AppSubPage) => void;
}> = ({ seasons, activeSubPage, setActiveSubPage }) => {
  const [selectedDivision, setSelectedDivision] = useState<string | undefined>(undefined);

  const getDivisions = () => {
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

  const getDivisionTier = (divisionName: string) => {
    const divisions = getDivisions().filter((d) => d.name === divisionName);
    console.log(divisions);
    if (divisions.length !== 1) {
      throw new Error(`The division name (${divisionName}) provided matches more than one tier.`);
    }

    return divisions[0].tier;
  };

  if (seasons.length === 0) {
    return null;
  }

  let body;
  if (activeSubPage === "Table") {
    console.log(selectedDivision);
    body = selectedDivision && (
      <Season selectedTier={getDivisionTier(selectedDivision)} seasons={seasons} />
    );
  } else if (activeSubPage === "Results") {
    body = null;
  }

  return (
    <>
      <DivisionFilter
        divisions={getDivisions()}
        selectedDivision={selectedDivision}
        selectDivision={(name) => {
          setActiveSubPage(name ? "Table" : "None");
          setSelectedDivision(name);
        }}
      />
      <Divider />
      {body}
    </>
  );
};

export { LeaguePage };
