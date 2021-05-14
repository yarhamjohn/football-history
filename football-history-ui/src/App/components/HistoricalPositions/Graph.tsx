import React, { FunctionComponent } from "react";
import { ResponsiveLine } from "@nivo/line";
import { Tooltip } from "./Tooltip";
import { Loader } from "semantic-ui-react";
import { useHistoricalPositionsGraph } from "./useHistoricalPositionsGraph";
import { HistoricalPositionRange } from "./index";
import { HistoricalPosition } from "../../shared/useFetchHistoricalPositions";

const HistoricalPositionsGraph: FunctionComponent<{
  isLoading: boolean;
  positions: HistoricalPosition[];
  range: HistoricalPositionRange;
}> = ({ isLoading, positions, range }) => {
  const { series, colors, yValues } = useHistoricalPositionsGraph(positions, range);

  return (
    <div style={{ height: "600px", position: "relative" }}>
      {isLoading && <Loader active style={{ position: "absolute", top: "50%", left: "50%" }} />}
      <div style={{ position: "absolute", top: 0, left: 0, height: "100%", width: "100%" }}>
        <ResponsiveLine
          data={series}
          colors={colors}
          margin={{ left: 25, bottom: 25, top: 10 }}
          yScale={{
            type: "linear",
            min: Math.min(...yValues),
            max: Math.max(...yValues),
            reverse: true,
          }}
          gridYValues={yValues}
          enableSlices="x"
          sliceTooltip={({ slice }) => {
            return <Tooltip points={slice.points} positions={positions} />;
          }}
          axisBottom={{
            orient: "bottom",
            tickSize: 5,
            tickPadding: 5,
            tickRotation: 0,
          }}
        />
      </div>
    </div>
  );
};

export { HistoricalPositionsGraph };
