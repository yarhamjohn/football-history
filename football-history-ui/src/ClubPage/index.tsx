import React, { FunctionComponent, useEffect, useState } from "react";
import { Divider } from "semantic-ui-react";
import { Club, useClubs } from "./useClubs";
import { LeagueTable } from "../components/LeagueTable";
import { useSeasons } from "./useSeasons";
import { ClubFilter } from "./ClubFilter";
import { SeasonFilter } from "./SeasonFilter";

const ClubPage: FunctionComponent = () => {
  const { seasons } = useSeasons();
  const { clubs } = useClubs();
  const [selectedClub, setSelectedClub] = useState<Club | undefined>(undefined);
  const [selectedSeason, setSelectedSeason] = useState<number | undefined>(undefined);

  useEffect(() => {
    if (seasons === undefined) {
      return;
    }

    setSelectedSeason(Math.max(...seasons.map((s) => s.startYear)));
  }, [selectedClub]);

  return (
    <div
      style={{
        display: "grid",
        gridTemplateColumns: "auto",
        gridTemplateRows: "auto auto",
        gridTemplateAreas: "'clubFilter' 'clubMain'",
      }}
    >
      <ClubFilter
        clubs={clubs}
        selectedClub={selectedClub}
        setSelectedClub={(selection: Club | undefined) => setSelectedClub(selection)}
        style={{ gridArea: "clubFilter" }}
      />
      <div style={{ gridArea: "clubMain", marginBottom: "1rem" }}>
        <Divider />
        {selectedClub !== undefined && seasons !== undefined && (
          <div
            style={{
              display: "grid",
              gridTemplateRows: "auto auto",
              gridTemplateColumns: "auto",
              gridTemplateAreas: "'seasonFilter' 'leagueTable'",
            }}
          >
            <SeasonFilter
              seasons={seasons}
              selectedSeason={selectedSeason}
              setSelectedSeason={(startYear) => setSelectedSeason(startYear)}
              style={{ gridArea: "seasonFilter", marginBottom: "1rem" }}
            />
            <LeagueTable
              club={selectedClub.name}
              seasonStartYear={selectedSeason}
              style={{ gridArea: "leagueTable" }}
            />
          </div>
        )}
      </div>
    </div>
  );
};

export { ClubPage };
