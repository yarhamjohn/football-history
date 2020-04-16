import React, { FunctionComponent, useState } from "react";
import { Divider, Dropdown, DropdownItemProps } from "semantic-ui-react";
import { Club, useClubs } from "./useClubs";

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
  const [selectedClub, setSelectedClub] = useState<Club>();

  const selectClub = (selection: any) => {
    if (isString(selection)) {
      const club = clubs.filter((c) => c.name === selection);
      if (club.length !== 1) {
        throw " Incorrect number of clubs";
      }
      setSelectedClub(club[0]);
    } else {
      throw "The selection was not a string type";
    }
  };

  return (
    <div
      style={{
        margin: "25px",
        display: "grid",
        gridTemplateColumns: "auto auto",
        gridTemplateRows: "auto auto",
        gridTemplateAreas: "'club filter' 'clubMain clubMain'",
      }}
    >
      <h1 style={{ margin: 0, gridArea: "club" }}>{selectedClub?.name}</h1>
      <Dropdown
        placeholder="Select Club"
        fluid
        search
        selection
        options={GetDropDownClubs(clubs)}
        style={{ gridArea: "filter" }}
        onChange={(_, data) => selectClub(data.value)}
      />
      <div style={{ gridArea: "clubMain" }}>
        <Divider />
      </div>
    </div>
  );
};

export { ClubPage };
