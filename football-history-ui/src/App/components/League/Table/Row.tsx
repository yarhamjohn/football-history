import React, { FunctionComponent, useEffect, useState } from "react";
import { Row } from "../../../shared/useFetchLeague";
import { Icon, Table } from "semantic-ui-react";
import { LeagueTableDrillDown } from "../DrillDown/DrillDown";
import { LeagueTableRowCell } from "./Cell";
import { Team } from "../../../shared/useFetchClubs";
import { CompetitionRules } from "../../../shared/useFetchCompetitions";
import { useLeagueTableRow } from "./useLeagueTableRow";

const LeagueTableRow: FunctionComponent<{
  row: Row;
  selectedClub: Team | undefined;
  seasonStartYear: number;
  numRows: number;
  rules: CompetitionRules;
  competitionId: number;
}> = ({ row, selectedClub, seasonStartYear, numRows, rules, competitionId }) => {
  const { bold, color } = useLeagueTableRow(row, selectedClub);
  const [showDrillDown, setShowDrillDown] = useState<boolean>(false);

  useEffect(() => {
    setShowDrillDown(false);
  }, [selectedClub, seasonStartYear]);

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
          {row.team}
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
