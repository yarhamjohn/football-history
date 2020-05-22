import React, { FunctionComponent } from "react";
import { useHistoricalPositions } from "../hooks/useHistoricalPositions";
import { ResponsiveLine, Layer } from "@nivo/line";

const HistoricalPositions: FunctionComponent<{ selectedClub: string }> = ({ selectedClub }) => {
  const { historicalPositionsState } = useHistoricalPositions(selectedClub, 2000, 2015);

  function getPositionData() {
    return historicalPositionsState.type === "HISTORICAL_POSITIONS_LOAD_SUCCEEDED"
      ? historicalPositionsState.positions
          .map((p) => {
            return { year: p.seasonStartYear, position: p.absolutePosition, status: p.status };
          })
          .sort()
      : [];
  }

  const getDates = (start: number, end: number) =>
    Array.from({ length: end - start }, (v, k) => k + start);

  function getData() {
    const dates = getDates(1999, 2016);
    const annotatedPositions = getPositionData();

    const positions = dates.map((d) => {
      return {
        x: d,
        y: annotatedPositions.some((p) => p.year === d)
          ? annotatedPositions.filter((p) => p.year === d)[0].position
          : null,
      };
    });

    let data = [
      {
        id: "positions",
        data: positions,
      },
    ];
    let colors = ["black"];

    return { data, colors };
  }

  const { data, colors } = getData();

  function getTicks() {
    return [1, 21, 45, 69, 92];
  }

  return (
    <div style={{ height: "500px" }}>
      <ResponsiveLine
        data={data}
        colors={colors}
        margin={{ left: 25, bottom: 25, top: 10 }}
        yScale={{ type: "linear", min: 1, max: 92, reverse: true }}
        gridYValues={getTicks()}
        axisBottom={{
          orient: "bottom",
          tickSize: 5,
          tickPadding: 5,
          tickRotation: 0,
        }}
      />
    </div>
  );
};

export { HistoricalPositions };
