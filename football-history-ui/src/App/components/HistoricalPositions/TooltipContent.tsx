import React, { FunctionComponent } from "react";
import { Point } from "@nivo/line";
import { useHistoricalPositionsTooltipContent } from "./useHistoricalPositionsTooltipContent";
import { HistoricalPosition } from "../../shared/useFetchHistoricalPositions";

const TooltipContent: FunctionComponent<{ point: Point; positions: HistoricalPosition[] }> = ({
  point,
  positions,
}) => {
  const {
    leaguePosition,
    leagueStatus,
    leagueName,
    color,
    absolutePosition,
    seasonStartYear,
  } = useHistoricalPositionsTooltipContent(point, positions);

  if (absolutePosition % 1 !== 0) {
    // The point is from one of the league boundary series
    return null;
  }

  return (
    <div
      key={point.id}
      style={{
        color: point.serieColor,
        padding: "12px 12px",
        display: "flex",
        flexDirection: "column",
        boxShadow: `0px 0px 10px ${color} inset`,
      }}
    >
      {color === null ? null : <h3 style={{ color: color.toString() }}>{leagueStatus}</h3>}
      <strong>{leagueName}</strong>
      <span>
        <strong>Position</strong>: {leaguePosition}
      </span>
      <span>
        <strong>Season</strong>: {seasonStartYear}-{seasonStartYear + 1}
      </span>
    </div>
  );
};

export { TooltipContent };
