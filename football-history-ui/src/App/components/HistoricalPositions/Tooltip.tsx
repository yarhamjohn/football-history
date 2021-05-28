import React, { FunctionComponent } from "react";
import { Point } from "@nivo/line";
import { TooltipContent } from "./TooltipContent";
import { HistoricalPosition } from "../../shared/useFetchHistoricalPositions";

const Tooltip: FunctionComponent<{
  points: Point[];
  positions: HistoricalPosition[];
}> = ({ points, positions }) => {
  return (
    <div
      style={{
        background: "white",
        border: "1px solid #ccc",
        borderRadius: "5px",
      }}
    >
      {points.map((point) =>
        point.data.y === null ? null : (
          <TooltipContent key={point.id} point={point} positions={positions} />
        )
      )}
    </div>
  );
};

export { Tooltip };
