import React, { FunctionComponent } from "react";
import { Dropdown, DropdownItemProps } from "semantic-ui-react";
import { Competition, useFetchCompetitions } from "../../shared/useFetchCompetitions";
import { Season } from "../../shared/useFetchSeasons";

const CompetitionFilter: FunctionComponent<{
  selectedSeason: Season;
  selectedCompetition: Competition | undefined;
  selectCompetition: (competition: Competition | undefined) => void;
}> = ({ selectedSeason, selectedCompetition, selectCompetition }) => {
  const competitions = useFetchCompetitions(selectedSeason.id);

  if (competitions.status !== "LOAD_SUCCESSFUL") {
    return null;
  }

  function createDropdown(): DropdownItemProps[] {
    if (competitions.status !== "LOAD_SUCCESSFUL") {
      return [];
    }

    return competitions.data.map((c) => {
      return {
        key: c.id,
        text: `${c.name} (${c.level})`,
        value: c.id,
      };
    });
  }

  function chooseCompetition(id: number | undefined) {
    if (competitions.status !== "LOAD_SUCCESSFUL") {
      return [];
    }

    const competition = competitions.data.filter((x) => x.id === id)[0];
    selectCompetition(competition);
  }

  return (
    <div
      style={{
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
      }}
    >
      {selectedCompetition === undefined ? (
        <p style={{ margin: "0 50px 0 0" }}>Select a competition from the dropdown.</p>
      ) : (
        <h1 style={{ margin: 0 }}>{selectedCompetition.name}</h1>
      )}
      <Dropdown
        placeholder="Select Division"
        clearable
        search
        selection
        options={createDropdown()}
        onChange={(_, data) =>
          chooseCompetition(isNaN(Number(data.value)) ? undefined : Number(data.value))
        }
        style={{ maxHeight: "25px" }}
      />
    </div>
  );
};

export { CompetitionFilter };
