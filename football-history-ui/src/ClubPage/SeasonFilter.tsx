import { Dropdown, DropdownItemProps, Icon } from "semantic-ui-react";
import React, { FunctionComponent } from "react";
import { Season } from "./useSeasons";
import { isNumber } from "../shared/functions";

const SeasonFilter: FunctionComponent<{
  seasons: Season[];
  selectedSeason: number | undefined;
  setSelectedSeason: (startYear: number) => void;
  style: React.CSSProperties;
}> = ({ seasons, selectedSeason, setSelectedSeason, style }) => {
  function GetDropdownSeasons(): DropdownItemProps[] {
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

  const selectSeasonStartYear = (selection: any) => {
    if (isNumber(selection)) {
      setSelectedSeason(selection);
    } else {
      throw new Error("An unexpected error occurred. The selection could not be processed.");
    }
  };

  const changeSeason = (nextSeason: number) => {
    if (seasons.some((s) => s.startYear === nextSeason)) {
      setSelectedSeason(nextSeason);
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
    <div style={{ ...style, display: "flex", alignItems: "center", color: "#00B5AD" }}>
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
        options={GetDropdownSeasons()}
        onChange={(_, data) => selectSeasonStartYear(data.value)}
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
