import React, { FunctionComponent, useEffect, useState } from "react";
import { Row } from "../../../shared/useFetchLeague";
import { Icon, Table } from "semantic-ui-react";
import { LeagueTableDrillDown } from "../DrillDown/DrillDown";
import { LeagueTableRowCell } from "./Cell";
import { useLeagueTableRow } from "./useLeagueTableRow";
import { selectTeam, Team } from "../../../shared/teamsSlice";
import { CompetitionRules } from "../../../shared/useFetchCompetitions";
import { useAppDispatch } from "../../../../reduxHooks";

const LeagueTableRow: FunctionComponent<{
  row: Row;
  selectedTeam: Team | undefined;
  seasonStartYear: number;
  numRows: number;
  rules: CompetitionRules;
  competitionId: number;
}> = ({ row, selectedTeam, seasonStartYear, numRows, rules, competitionId }) => {
  const dispatch = useAppDispatch();
  const { bold, color } = useLeagueTableRow(row, selectedTeam);
  const [showDrillDown, setShowDrillDown] = useState<boolean>(false);

  useEffect(() => {
    setShowDrillDown(false);
  }, [selectedTeam, seasonStartYear]);

  const switchToTeam = (teamId: number) => {
    dispatch(selectTeam(teamId));
    // TODO: switch tab
  };

  return (
    <>
      <Table.Row
        style={{
          cursor: "pointer",
        }}
        onClick={() => setShowDrillDown(!showDrillDown)}
      >
        <LeagueTableRowCell bold={bold} color={color}>
          {showDrillDown ? <Icon name="chevron down" /> : <Icon name="chevron right" />}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={bold} color={color}>
          {row.position}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={bold} color={color}>
          <a onClick={() => switchToTeam(row.teamId)}>{row.team}</a>
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={bold} color={color}>
          {row.played}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={bold} color={color}>
          {row.won}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={bold} color={color}>
          {row.drawn}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={bold} color={color}>
          {row.lost}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={bold} color={color}>
          {row.goalsFor}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={bold} color={color}>
          {row.goalsAgainst}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={bold} color={color}>
          {row.goalDifference}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={bold} color={color}>
          {Number(Math.round(parseFloat(row.goalAverage + "e4")) + "e-4")}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={bold} color={color}>
          {Number(Math.round(parseFloat(row.pointsPerGame + "e2")) + "e-2")}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={bold} color={color}>
          {row.points}
          {row.pointsDeducted > 0 ? " *" : ""}
        </LeagueTableRowCell>
        <LeagueTableRowCell bold={bold} color={color}>
          {row.status}
        </LeagueTableRowCell>
      </Table.Row>
      {showDrillDown ? (
        <LeagueTableDrillDown
          teamId={row.teamId}
          competitionId={competitionId}
          numRows={numRows}
          rules={rules}
        />
      ) : null}
    </>
  );
};

export { LeagueTableRow };
