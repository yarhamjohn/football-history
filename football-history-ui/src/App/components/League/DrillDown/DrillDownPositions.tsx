import React, { FunctionComponent } from "react";
import { LeaguePosition } from "../../../shared/useFetchLeaguePositions";
import { ResponsiveLine } from "@nivo/line";
import { CompetitionRules } from "../../../shared/useFetchCompetitions";
import { useDrillDownPositions } from "./useDrillDownPositions";

const LeagueTableDrillDownPositions: FunctionComponent<{
  positions: LeaguePosition[];
  rules: CompetitionRules;
  numRows: number;
}> = ({ positions, rules, numRows }) => {
  const { data, colors, gridYValues, gridXValues } = useDrillDownPositions(
    positions,
    rules,
    numRows
  );

  return (
    <div style={{ position: "absolute", top: 0, left: 0, height: "100%", width: "100%" }}>
      <ResponsiveLine
        data={data}
        colors={colors}
        margin={{ left: 25, bottom: 10, top: 10 }}
        yScale={{ type: "linear", min: 1, max: numRows, reverse: true }}
        enablePoints={false}
        gridYValues={gridYValues}
        gridXValues={gridXValues}
        axisBottom={null}
      />
    </div>
  );
};

export { LeagueTableDrillDownPositions };
