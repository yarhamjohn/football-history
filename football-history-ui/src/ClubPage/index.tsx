import React, { FunctionComponent } from "react";
import { Divider, Dropdown, DropdownItemProps } from "semantic-ui-react";

const ClubPage: FunctionComponent = () => {
  const clubs: DropdownItemProps[] = [{ key: "NOR", text: "Norwich City" }, { key: "ARS", text: "Arsenal" }];
  return <div style={{
    margin: "25px",
    display: "grid",
    gridTemplateColumns: "auto auto",
    gridTemplateRows: "auto auto",
    gridTemplateAreas: "'club filter' 'clubMain clubMain'",
  }}>
    <h1 style={{margin: 0, gridArea:"club"}}>Arsenal</h1>
    <Dropdown placeholder="Select Club" fluid search selection options={clubs} style={{gridArea: "filter"}}/>
    <div style={{gridArea: "clubMain"}} >
      <Divider />
    </div>
  </div>;
};

export { ClubPage };