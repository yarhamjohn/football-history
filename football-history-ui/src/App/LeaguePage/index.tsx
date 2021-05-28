import React, { FunctionComponent, useEffect, useState } from "react";
import { Divider } from "semantic-ui-react";
import { Season } from "../shared/useFetchSeasons";
import { CompetitionFilter } from "../components/Filters/CompetitionFilter";
import { AppSubPage } from "../App";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { Matches } from "./Matches";
import { Competition } from "../shared/useFetchCompetitions";
import { League } from "../components/League";

const LeaguePage: FunctionComponent<{
  seasons: Season[];
  activeSubPage: AppSubPage;
  setActiveSubPage: (subPage: AppSubPage) => void;
}> = ({ seasons, activeSubPage, setActiveSubPage }) => {
  const [selectedCompetition, setSelectedCompetition] = useState<Competition | undefined>(
    undefined
  );
  const [selectedSeason, setSelectedSeason] = useState<Season | undefined>(undefined);

  useEffect(() => {
    const season = seasons.reduce(function (prev, current) {
      return prev.startYear > current.startYear ? prev : current;
    });

    setSelectedSeason(season);
  }, [seasons]);

  let body;
  if (activeSubPage === "Table") {
    body = selectedCompetition && (
      <>
        <SeasonFilter
          seasons={seasons}
          selectedSeason={selectedSeason}
          selectSeason={(startYear) => setSelectedSeason(startYear)}
        />
        {selectedSeason && (
          <League
            props={{
              season: selectedSeason,
              competition: selectedCompetition,
            }}
          />
        )}
      </>
    );
  } else if (activeSubPage === "Results") {
    body = selectedCompetition && (
      <div style={{ display: "grid", gridGap: "1rem" }}>
        <SeasonFilter
          seasons={seasons}
          selectedSeason={selectedSeason}
          selectSeason={(startYear) => setSelectedSeason(startYear)}
        />
        {selectedSeason ? <Matches competitionId={selectedCompetition.id} /> : null}
      </div>
    );
  }

  return (
    <>
      {selectedSeason && (
        <CompetitionFilter
          selectedSeason={selectedSeason}
          selectedCompetition={selectedCompetition}
          selectCompetition={(name) => {
            setActiveSubPage(name ? "Table" : "None");
            setSelectedCompetition(name);
          }}
        />
      )}
      <Divider />
      {body}
    </>
  );
};

export { LeaguePage };
