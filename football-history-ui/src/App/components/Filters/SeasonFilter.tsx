import { Dropdown, DropdownItemProps, Icon } from "semantic-ui-react";
import React, { FunctionComponent } from "react";
import { useAppDispatch, useAppSelector } from "../../../reduxHooks";
import { Season, selectSeason } from "../../shared/seasonsSlice";

const SeasonFilter: FunctionComponent = () => {
  const dispatch = useAppDispatch();
  const seasonState = useAppSelector((state) => state.season);

  function createDropdown(): DropdownItemProps[] {
    return seasonState.seasons
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
    if (seasonState.seasons.some((s) => s.startYear === nextSeason.startYear)) {
      dispatch(selectSeason(nextSeason));
    } else {
      return;
    }
  };

  const forwardOneSeason = () => {
    if (seasonState.selectedSeason !== undefined) {
      const nextStartYear = seasonState.selectedSeason.startYear + 1;
      const nextSeason = seasonState.seasons.filter((x) => x.startYear === nextStartYear);

      if (nextSeason.length === 1) {
        changeSeason(nextSeason[0]);
      }
    }
  };

  const backOneSeason = () => {
    if (seasonState.selectedSeason !== undefined) {
      const previousStartYear = seasonState.selectedSeason.startYear - 1;
      const previousSeason = seasonState.seasons.filter((x) => x.startYear === previousStartYear);

      if (previousSeason.length === 1) {
        changeSeason(previousSeason[0]);
      }
    }
  };

  function chooseSeason(startYear: number | undefined) {
    const season = seasonState.seasons.filter((x) => x.startYear === startYear)[0];
    dispatch(selectSeason(season));
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
        value={seasonState.selectedSeason?.startYear}
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
