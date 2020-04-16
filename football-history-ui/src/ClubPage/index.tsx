import React, { FunctionComponent } from "react";
import { Divider, Dropdown, DropdownItemProps } from "semantic-ui-react";
import { Club, useClubs } from "./useClubs";

function GetDropDownClubs(clubs: Club[]): DropdownItemProps[] {
  return clubs.map((c) => {
    return { key: c.name, text: `${c.name} (${c.abbreviation})` };
  });
}

const ClubPage: FunctionComponent = () => {
  const clubs = useClubs();

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
      <h1 style={{ margin: 0, gridArea: "club" }}>Arsenal</h1>
      <Dropdown
        placeholder="Select Club"
        fluid
        search
        selection
        options={GetDropDownClubs(clubs)}
        style={{ gridArea: "filter" }}
      />
      <div style={{ gridArea: "clubMain" }}>
        <Divider />
      </div>
    </div>
  );
};

export { ClubPage };
