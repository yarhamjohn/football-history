import React, { FunctionComponent, useEffect, useState } from "react";
import { Divider, Dropdown, DropdownItemProps } from "semantic-ui-react";
import { Club, useClubs } from "./useClubs";
import { LeagueTable } from "../components/LeagueTable";
import { Season, useSeasons } from "./useSeasons";

function GetDropdownClubs(clubs: Club[]): DropdownItemProps[] {
  return clubs.map((c) => {
    return {
      key: c.name,
      text: c.name,
      value: c.name,
    };
  });
}

function GetDropdownSeasons(seasons: Season[]): DropdownItemProps[] {
  return seasons.map((s) => {
    return {
      key: s.startYear,
      text: `${s.startYear} - ${s.endYear}`,
      value: s.startYear,
    };
  });
}

function isString(x: any): x is string {
  return typeof x === "string";
}

function isNumber(x: any): x is number {
  return typeof x === "number";
}

const ClubPage: FunctionComponent = () => {
  const { clubs } = useClubs();
  const { seasons } = useSeasons();
  const [selectedClub, setSelectedClub] = useState<Club | undefined>(undefined);
  const [selectedSeasonStartYear, setSelectedSeasonStartYear] = useState<number | undefined>(
    undefined
  );

  const selectClub = (selection: any) => {
    if (isString(selection)) {
      const club = clubs.filter((c) => c.name === selection);
      if (club.length !== 1) {
        throw new Error(
          `Clubs should be unique but there were ${
            club.length
          } clubs that matches the selection ${club.join(",")}.`
        );
      }
      setSelectedClub(club[0]);
    } else {
      throw new Error("An unexpected error occurred. The selection could not be processed.");
    }
  };

  const selectSeasonStartYear = (selection: any) => {
    if (isNumber(selection)) {
      setSelectedSeasonStartYear(selection);
    } else {
      throw new Error("An unexpected error occurred. The selection could not be processed.");
    }
  };

  useEffect(() => {
    if (seasons === undefined) {
      return undefined;
    }

    return setSelectedSeasonStartYear(Math.max(...seasons.map((s) => s.startYear)));
  }, [selectedClub]);

  return (
    <div
      style={{
        marginTop: "1rem",
        display: "grid",
        gridTemplateColumns: "auto minmax(100px, auto) auto",
        gridTemplateRows: "auto auto",
        gridTemplateAreas: "'clubTopLeft filter clubTopRight' 'clubMain clubMain clubMain'",
      }}
    >
      <Dropdown
        placeholder="Select Club"
        search
        selection
        options={GetDropdownClubs(clubs)}
        style={{ gridArea: "filter" }}
        onChange={(_, data) => selectClub(data.value)}
      />
      <div style={{ gridArea: "clubMain" }}>
        <Divider />
        {selectedClub === undefined ? (
          <p>
            Select a club from the dropdown. The list contains all clubs to have featured in the
            Football League or Premier League since 1992.
          </p>
        ) : (
          <>
            <h1>{selectedClub.name}</h1>
            <LeagueTable club={selectedClub.name} seasonStartYear={selectedSeasonStartYear} />
            {seasons !== undefined && (
              <Dropdown
                placeholder="Select Season"
                fluid
                selection
                options={GetDropdownSeasons(seasons)}
                onChange={(_, data) => selectSeasonStartYear(data.value)}
                value={selectedSeasonStartYear}
              />
            )}
          </>
        )}
      </div>
    </div>
  );
};

export { ClubPage };
