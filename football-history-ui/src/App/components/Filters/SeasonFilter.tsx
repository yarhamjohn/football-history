import { Dropdown, DropdownItemProps, Icon } from "semantic-ui-react";
import React, { FunctionComponent } from "react";
import { Season } from "../../shared/useFetchSeasons";

const SeasonFilter: FunctionComponent<{
  seasons: Season[];
  selectedSeason: Season | undefined;
  selectSeason: (season: Season | undefined) => void;
}> = ({ seasons, selectedSeason, selectSeason }) => {
  function createDropdown(): DropdownItemProps[] {
    return seasons
      .slice()
      .sort((a, b) => b.startYear - a.startYear)
      .map((s) => {
        return {
          key: s.startYear,
          text: `${s.startYear} - ${s.endYear}`,
          value: s.startYear,
        };
      });
  }

  const changeSeason = (nextSeason: Season) => {
    if (seasons.some((s) => s.startYear === nextSeason.startYear)) {
      selectSeason(nextSeason);
    } else {
      return;
    }
  };

  const forwardOneSeason = () => {
    if (selectedSeason !== undefined) {
      const nextStartYear = selectedSeason.startYear + 1;
      const nextSeason = seasons.filter((x) => x.startYear === nextStartYear);

      if (nextSeason.length === 1) {
        changeSeason(nextSeason[0]);
      }
    }
  };

  const backOneSeason = () => {
    if (selectedSeason !== undefined) {
      const previousStartYear = selectedSeason.startYear - 1;
      const previousSeason = seasons.filter((x) => x.startYear === previousStartYear);

      if (previousSeason.length === 1) {
        changeSeason(previousSeason[0]);
      }
    }
  };

  function chooseSeason(startYear: number | undefined) {
    const season = seasons.filter((x) => x.startYear === startYear)[0];
    selectSeason(season);
  }

  return (
    <div style={{ display: "flex", alignItems: "center", color: "#00B5AD", marginBottom: "2rem" }}>
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
          chooseSeason(isNaN(Number(data.value)) ? undefined : Number(data.value))
        }
        value={selectedSeason?.startYear}
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
