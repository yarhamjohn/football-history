import React, { FunctionComponent, useEffect } from "react";
import { Dropdown, DropdownItemProps } from "semantic-ui-react";
import { useAppDispatch, useAppSelector } from "../../../reduxHooks";
import { fetchTeams, selectTeam } from "../../shared/teamsSlice";

const ClubFilter: FunctionComponent = () => {
  const dispatch = useAppDispatch();
  const teamState = useAppSelector((state) => state.team);

  useEffect(() => {
    dispatch(fetchTeams());
  }, [dispatch]);

  function createDropdown(): DropdownItemProps[] {
    return teamState.teams.map((c) => {
      return {
        key: c.id,
        text: c.name,
        value: c.id,
      };
    });
  }

  function chooseTeam(id: number | undefined) {
    const team = teamState.teams.filter((x) => x.id === id)[0];
    dispatch(selectTeam(team));
  }

  return (
    <div
      style={{
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
      }}
    >
      {teamState.selectedTeam === undefined ? (
        <p style={{ margin: "0 50px 0 0" }}>
          Select a club from the dropdown. The list contains all clubs to have featured in the
          Football League or Premier League since 1992.
        </p>
      ) : (
        <h1 style={{ margin: 0 }}>{teamState.selectedTeam.name}</h1>
      )}
      <Dropdown
        placeholder={teamState.selectedTeam?.name ?? "Select Club"}
        clearable
        search
        selection
        options={createDropdown()}
        onChange={(_, data) =>
          chooseTeam(isNaN(Number(data.value)) ? undefined : Number(data.value))
        }
        style={{ maxHeight: "25px" }}
      />
    </div>
  );
};

export { ClubFilter };
