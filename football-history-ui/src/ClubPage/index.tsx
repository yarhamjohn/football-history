import React, { FunctionComponent, useState } from "react";
import { Divider, Dropdown, DropdownItemProps } from "semantic-ui-react";
import { Club, useClubs } from "./useClubs";
import { LeagueTable } from "../components/LeagueTable";
import { useSeasons } from "./useSeasons";

function GetDropDownClubs(clubs: Club[]): DropdownItemProps[] {
  return clubs.map((c) => {
    return {
      key: c.name,
      text: c.name,
      value: c.name,
    };
  });
}

function isString(x: any): x is string {
  return typeof x === "string";
}

const ClubPage: FunctionComponent = () => {
  const { clubs } = useClubs();
  const { seasons } = useSeasons();
  const [selectedClub, setSelectedClub] = useState<Club | undefined>(undefined);

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

  const getLastSeason = () => {
    if (seasons === undefined) {
      return undefined;
    }

    return Math.max(...seasons.map((s) => s.startYear));
  };

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
        options={GetDropDownClubs(clubs)}
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
            <LeagueTable club={selectedClub.name} seasonStartYear={getLastSeason()} />
          </>
        )}
      </div>
    </div>
  );
};

export { ClubPage };
