import React, { FunctionComponent } from "react";
import { Dropdown, DropdownItemProps } from "semantic-ui-react";
import { isString } from "../../shared/functions";
import { Division } from "../../hooks/useSeasons";

const DivisionFilter: FunctionComponent<{
  divisions: Division[];
  selectedDivision: Division | undefined;
  setSelectedDivision: (selectedDivision: Division | undefined) => void;
}> = ({ divisions, selectedDivision, setSelectedDivision }) => {
  function GetDropdownDivisions(divisions: Division[]): DropdownItemProps[] {
    return divisions.map((c) => {
      return {
        key: c.name,
        text: `${c.name} (${c.tier})`,
        value: c.name,
      };
    });
  }

  const selectDivision = (selection: any) => {
    if (isString(selection)) {
      if (selection === "") {
        setSelectedDivision(undefined);
        return;
      }

      const division = divisions.filter((c) => c.name === selection);
      if (division.length !== 1) {
        throw new Error(
          `Divisions should be unique but there were ${
            division.length
          } divisions that match the selection ${division.join(",")}.`
        );
      }
      setSelectedDivision(division[0]);
    } else {
      throw new Error(
        `An unexpected error occurred. The selection (${selection.toString()}) could not be processed.`
      );
    }
  };

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
        <h1 style={{ margin: 0 }}>{selectedDivision.name}</h1>
      )}
      <Dropdown
        placeholder="Select Division"
        clearable
        search
        selection
        options={GetDropdownDivisions(divisions)}
        onChange={(_, data) => selectDivision(data.value)}
        style={{ maxHeight: "25px" }}
      />
    </div>
  );
};

export { DivisionFilter };
