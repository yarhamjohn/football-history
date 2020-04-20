import React, { FunctionComponent, useEffect, useState } from "react";
import { Divider, Dropdown, DropdownItemProps, Icon } from "semantic-ui-react";
import { Club, useClubs } from "./useClubs";
import { LeagueTable } from "../components/LeagueTable";
import { Season, useSeasons } from "./useSeasons";
import { ClubFilter } from "./ClubFilter";

function GetDropdownSeasons(seasons: Season[]): DropdownItemProps[] {
  return seasons.map((s) => {
    return {
      key: s.startYear,
      text: `${s.startYear} - ${s.endYear}`,
      value: s.startYear,
    };
  });
}

function isNumber(x: any): x is number {
  return typeof x === "number";
}

const ClubPage: FunctionComponent = () => {
  const { seasons } = useSeasons();
  const [selectedClub, setSelectedClub] = useState<Club | undefined>(undefined);
  const [selectedSeasonStartYear, setSelectedSeasonStartYear] = useState<number | undefined>(
    undefined
  );

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
        display: "grid",
        gridTemplateColumns: "auto",
        gridTemplateRows: "auto auto",
        gridTemplateAreas: "'clubFilter' 'clubMain'",
      }}
    >
      <ClubFilter
        selectedClub={selectedClub}
        setSelectedClub={(selection: Club | undefined) => setSelectedClub(selection)}
        style={{ gridArea: "clubFilter" }}
      />
      <div style={{ gridArea: "clubMain" }}>
        <Divider />
        {selectedClub !== undefined && (
          <div
            style={{
              display: "grid",
              gridTemplateRows: "auto auto auto",
              gridTemplateColumns: "25px auto 25px",
              gridTemplateAreas: "'name name name' 'left filter right' 'table table table'",
            }}
          >
            {seasons !== undefined && (
              <>
                <Icon name="caret left" size="big" style={{ gridArea: "left" }} />
                <Dropdown
                  placeholder="Select Season"
                  fluid
                  selection
                  options={GetDropdownSeasons(seasons)}
                  onChange={(_, data) => selectSeasonStartYear(data.value)}
                  value={selectedSeasonStartYear}
                  style={{ gridArea: "filter" }}
                />
                <Icon name="caret right" size="big" style={{ gridArea: "right" }} />
              </>
            )}
            <LeagueTable
              club={selectedClub.name}
              seasonStartYear={selectedSeasonStartYear}
              style={{ gridArea: "table" }}
            />
          </div>
        )}{" "}
      </div>
    </div>
  );
};

export { ClubPage };
