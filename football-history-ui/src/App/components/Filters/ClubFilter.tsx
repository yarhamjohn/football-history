import React, { FunctionComponent } from "react";
import { Dropdown, DropdownItemProps } from "semantic-ui-react";
import { Team } from "../../shared/useFetchClubs";

const ClubFilter: FunctionComponent<{
  clubs: Team[];
  selectedClub: Team | undefined;
  selectClub: (club: Team | undefined) => void;
}> = ({ clubs, selectedClub, selectClub }) => {
  function createDropdown(): DropdownItemProps[] {
    return clubs.map((c) => {
      return {
        key: c.id,
        text: c.name,
        value: c.id,
      };
    });
  }

  function chooseClub(id: number | undefined) {
    const club = clubs.filter((x) => x.id === id)[0];
    selectClub(club);
  }

  return (
    <div
      style={{
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
      }}
    >
      {selectedClub === undefined ? (
        <p style={{ margin: "0 50px 0 0" }}>
          Select a club from the dropdown. The list contains all clubs to have featured in the
          Football League or Premier League since 1992.
        </p>
      ) : (
        <h1 style={{ margin: 0 }}>{selectedClub.name}</h1>
      )}
      <Dropdown
        placeholder="Select Club"
        clearable
        search
        selection
        options={createDropdown()}
        onChange={(_, data) =>
          chooseClub(isNaN(Number(data.value)) ? undefined : Number(data.value))
        }
        style={{ maxHeight: "25px" }}
      />
    </div>
  );
};

export { ClubFilter };
