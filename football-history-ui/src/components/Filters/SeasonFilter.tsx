import { Dropdown, DropdownItemProps, Icon } from "semantic-ui-react";
import React, { FunctionComponent } from "react";
import { Season } from "../../hooks/useSeasons";

const SeasonFilter: FunctionComponent<{
  seasons: Season[];
  selectedSeason: number | undefined;
  selectSeason: (startYear: number | undefined) => void;
}> = ({ seasons, selectedSeason, selectSeason }) => {
  function createDropdown(): DropdownItemProps[] {
    return seasons
      .sort((a, b) => b.startYear - a.startYear)
      .map((s) => {
        return {
          key: s.startYear,
          text: `${s.startYear} - ${s.endYear}`,
          value: s.startYear,
        };
      });
  }

  const changeSeason = (nextSeason: number) => {
    if (seasons.some((s) => s.startYear === nextSeason)) {
      selectSeason(nextSeason);
    } else {
      return;
    }
  };

  const forwardOneSeason = () => {
    selectedSeason !== undefined && changeSeason(selectedSeason + 1);
  };

  const backOneSeason = () => {
    selectedSeason !== undefined && changeSeason(selectedSeason - 1);
  };

  return (
    <div style={{ display: "flex", alignItems: "center", color: "#00B5AD" }}>
      <Icon
        name="caret left"
        size="huge"
        onClick={() => backOneSeason()}
        style={{ cursor: "pointer" }}
      />
      <Dropdown
        placeholder="Select Season"
        fluid
        selection
        options={createDropdown()}
        onChange={(_, data) =>
          selectSeason(isNaN(Number(data.value)) ? undefined : Number(data.value))
        }
        value={selectedSeason}
        style={{ gridArea: "filter" }}
      />
      <Icon
        name="caret right"
        size="huge"
        onClick={() => forwardOneSeason()}
        style={{ cursor: "pointer" }}
      />
    </div>
  );
};

export { SeasonFilter };
