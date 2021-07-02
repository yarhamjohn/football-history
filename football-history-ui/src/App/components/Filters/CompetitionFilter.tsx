import React, { FunctionComponent, useEffect } from "react";
import { Dropdown, DropdownItemProps } from "semantic-ui-react";
import { useAppDispatch, useAppSelector } from "../../../reduxHooks";
import { fetchCompetitionsBySeasonId, selectCompetition } from "../../shared/competitionsSlice";

const CompetitionFilter: FunctionComponent = () => {
  const dispatch = useAppDispatch();
  const seasonState = useAppSelector((state) => state.season);
  const competitionState = useAppSelector((state) => state.competition);

  useEffect(() => {
    if (seasonState.selectedSeason !== undefined) {
      dispatch(fetchCompetitionsBySeasonId(seasonState.selectedSeason.id));
    }
  }, [seasonState.selectedSeason, dispatch]);

  if (competitionState.status !== "LOADED") {
    return null;
  }

  function createDropdown(): DropdownItemProps[] {
    if (competitionState.status !== "LOADED") {
      return [];
    }

    return competitionState.competitions.map((c) => {
      return {
        key: c.id,
        text: `${c.name} (${c.level})`,
        value: c.id,
      };
    });
  }

  function chooseCompetition(id: number | undefined) {
    if (competitionState.status !== "LOADED") {
      return [];
    }

    const competition = competitionState.competitions.filter((x) => x.id === id)[0];
    dispatch(selectCompetition(competition));
  }

  return (
    <div
      style={{
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
      }}
    >
      {competitionState.selectedCompetition === undefined ? (
        <p style={{ margin: "0 50px 0 0" }}>Select a competition from the dropdown.</p>
      ) : (
        <h1 style={{ margin: 0 }}>{competitionState.selectedCompetition.name}</h1>
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
