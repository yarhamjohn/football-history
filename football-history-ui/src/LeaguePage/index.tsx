import React, { FunctionComponent, useEffect, useState } from "react";
import { Divider } from "semantic-ui-react";
import { Season } from "../hooks/useSeasons";
import { DivisionFilter } from "../components/Filters/LeagueFilter";
import { AppSubPage } from "../App";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { LeagueSeason } from "./LeagueSeason";

const LeaguePage: FunctionComponent<{
  seasons: Season[];
  activeSubPage: AppSubPage;
  setActiveSubPage: (subPage: AppSubPage) => void;
}> = ({ seasons, activeSubPage, setActiveSubPage }) => {
  const [selectedDivision, setSelectedDivision] = useState<string | undefined>(undefined);
  const [selectedSeason, setSelectedSeason] = useState<number | undefined>(undefined);

  useEffect(() => {
    setSelectedSeason(Math.max(...seasons.map((s) => s.startYear)));
  }, [seasons]);

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
    body = (
      <SeasonFilter
        seasons={seasons}
        selectedSeason={selectedSeason}
        selectSeason={(startYear) => setSelectedSeason(startYear)}
      />
    );
  } else if (activeSubPage === "Results") {
    body = selectedDivision && (
      <div style={{ display: "grid", gridGap: "1rem" }}>
        <SeasonFilter
          seasons={seasons}
          selectedSeason={selectedSeason}
          selectSeason={(startYear) => setSelectedSeason(startYear)}
        />
        {selectedSeason && getDivisionTier(selectedDivision) ? (
          <LeagueSeason
            selectedSeason={selectedSeason}
            selectedTier={getDivisionTier(selectedDivision)}
          />
        ) : null}
      </div>
    );
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
