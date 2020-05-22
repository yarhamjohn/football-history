import React, { FunctionComponent } from "react";
import { useHistoricalPositions } from "../hooks/useHistoricalPositions";
import { ResponsiveLine, Layer, Serie } from "@nivo/line";

const HistoricalPositions: FunctionComponent<{ selectedClub: string }> = ({ selectedClub }) => {
  const { historicalPositionsState } = useHistoricalPositions(selectedClub, 2000, 2015);

  function relegatedInSeason(season: number) {
    if (historicalPositionsState.type !== "HISTORICAL_POSITIONS_LOAD_SUCCEEDED") {
      return false;
    }
    const positions = historicalPositionsState.positions.filter(
      (p) => p.seasonStartYear === season
    );

    if (positions.length !== 1) {
      return false;
    }

    return positions[0].status === "Relegated";
  }
  function promotedInSeason(season: number) {
    if (historicalPositionsState.type !== "HISTORICAL_POSITIONS_LOAD_SUCCEEDED") {
      return false;
    }
    const positions = historicalPositionsState.positions.filter(
      (p) => p.seasonStartYear === season
    );

    if (positions.length !== 1) {
      return false;
    }

    return (
      positions[0].status === "Promoted" ||
      positions[0].status === "Champions" ||
      positions[0].status === "PlayOff Winner"
    );
  }

  function clubIsInTier(seasonStartYear: number, tier: number) {
    if (historicalPositionsState.type !== "HISTORICAL_POSITIONS_LOAD_SUCCEEDED") {
      return false;
    }

    return historicalPositionsState.positions.some(
      (p) => p.seasonStartYear === seasonStartYear && p.tier === tier
    );
  }

  function clubWasRelegatedToTier(season: number, tier: number) {
    return relegatedInSeason(season) && clubIsInTier(season, tier - 1);
  }

  function clubWasPromotedToTier(season: number, tier: number) {
    return promotedInSeason(season) && clubIsInTier(season, tier + 1);
  }

  const getDates = (start: number, end: number) =>
    Array.from({ length: end - start }, (v, k) => k + start);

  function getData() {
    let series: Serie[] = [];
    let colors = ["black", "red", "blue", "green"];

    if (historicalPositionsState.type !== "HISTORICAL_POSITIONS_LOAD_SUCCEEDED") {
      return { series, colors };
    }

    const allDates = getDates(1999, 2016);
    const tiers = Array.from(new Set(historicalPositionsState.positions.map((p) => p.tier)));
    series = tiers.map((t) => {
      return {
        id: `tier${t}Positions`,
        data: allDates.map((d) => {
          return {
            x: d,
            y:
              clubIsInTier(d, t) || clubWasRelegatedToTier(d, t) || clubWasPromotedToTier(d, t)
                ? historicalPositionsState.positions.filter((p) => p.seasonStartYear === d)[0]
                    .absolutePosition
                : null,
          };
        }),
      };
    });

    return { series, colors };
  }

  const { series, colors } = getData();

  function getTicks() {
    return [1, 21, 45, 69, 92];
  }

  return (
    <div style={{ height: "500px" }}>
      <ResponsiveLine
        data={series}
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
