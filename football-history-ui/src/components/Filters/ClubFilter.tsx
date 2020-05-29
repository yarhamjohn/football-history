import React, { FunctionComponent } from "react";
import { Dropdown, DropdownItemProps } from "semantic-ui-react";
import { Club } from "../../hooks/useClubs";

const ClubFilter: FunctionComponent<{
  clubs: Club[];
  selectedClub: string | undefined;
  selectClub: (name: string | undefined) => void;
}> = ({ clubs, selectedClub, selectClub }) => {
  function createDropdown(): DropdownItemProps[] {
    return clubs.map((c) => {
      return {
        key: c.name,
        text: c.name,
        value: c.name,
      };
    });
  }

  return (
    <div
      style={{
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
      }}
    >
      {selectedClub === undefined || selectedClub === "" ? (
        <p style={{ margin: "0 50px 0 0" }}>
          Select a club from the dropdown. The list contains all clubs to have featured in the
          Football League or Premier League since 1992.
        </p>
      ) : (
        <h1 style={{ margin: 0 }}>{selectedClub}</h1>
      )}
      <Dropdown
        placeholder="Select Club"
        clearable
        search
        selection
        options={createDropdown()}
        onChange={(_, data) => selectClub(data.value?.toString())}
        style={{ maxHeight: "25px" }}
      />
    </div>
  );
};

export { ClubFilter };
