import React, { FunctionComponent } from "react";
import { Dropdown, DropdownItemProps } from "semantic-ui-react";
import { isString } from "../../shared/functions";
import { Division } from "../../hooks/useSeasons";

const DivisionFilter: FunctionComponent<{
  divisions: Division[];
  selectedDivision: string | undefined;
  selectDivision: (name: string | undefined) => void;
}> = ({ divisions, selectedDivision, selectDivision }) => {
  function createDropdown(): DropdownItemProps[] {
    return divisions.map((c) => {
      return {
        key: c.name,
        text: `${c.name} (${c.tier})`,
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
      {selectedDivision === undefined ? (
        <p style={{ margin: "0 50px 0 0" }}>Select a division from the dropdown.</p>
      ) : (
        <h1 style={{ margin: 0 }}>{selectedDivision}</h1>
      )}
      <Dropdown
        placeholder="Select Division"
        clearable
        search
        selection
        options={createDropdown()}
        onChange={(_, data) => selectDivision(data.value?.toString())}
        style={{ maxHeight: "25px" }}
      />
    </div>
  );
};

export { DivisionFilter };
