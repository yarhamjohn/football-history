import React, { FunctionComponent, useState } from "react";
import { useHistoricalPositions } from "../hooks/useHistoricalPositions";
import { ResponsiveLine, Serie } from "@nivo/line";
import { Season as SeasonType } from "../hooks/useSeasons";
import { YearSlider } from "./Filters/YearSlider";

const HistoricalPositions: FunctionComponent<{
  selectedClub: string;
  seasons: SeasonType[];
}> = ({ selectedClub, seasons }) => {
  function getFirstSeasonStartYear() {
    return Math.min(...seasons.map((s) => s.startYear));
  }

  function getLastSeasonEndYear() {
    return Math.max(...seasons.map((s) => s.endYear));
  }

  const [selectedFilterRange, setSelectedFilterRange] = useState<number[]>([
    getFirstSeasonStartYear(),
    getLastSeasonEndYear(),
  ]);

  const { historicalPositionsState } = useHistoricalPositions(selectedClub, selectedFilterRange);

  const getDates = (start: number, end: number) =>
    Array.from({ length: end - start }, (v, k) => k + start);

  function getData() {
    let series: Serie[] = [];
    let colors = ["black", "#75B266", "#BFA67F", "#B26694"];

    if (historicalPositionsState.type !== "HISTORICAL_POSITIONS_LOAD_SUCCEEDED") {
      return { series, colors };
    }

    const allDates = getDates(selectedFilterRange[0] - 1, selectedFilterRange[1] + 1);

    series = [
      {
        id: "positions",
        data: allDates.map((d) => {
          return {
            x: d,
            y: historicalPositionsState.positions.some((p) => p.seasonStartYear === d)
              ? historicalPositionsState.positions.filter((p) => p.seasonStartYear === d)[0]
                  .absolutePosition
              : null,
          };
        }),
      },
      {
        id: "tier1-tier2",
        data: [
          { x: Math.min(...allDates), y: 20 },
          { x: Math.max(...allDates), y: 20 },
        ],
      },
      {
        id: "tier2-tier3",
        data: [
          { x: Math.min(...allDates), y: 44 },
          { x: Math.max(...allDates), y: 44 },
        ],
      },
      {
        id: "tier3-tier4",
        data: [
          { x: Math.min(...allDates), y: 68 },
          { x: Math.max(...allDates), y: 68 },
        ],
      },
    ];

    return { series, colors };
  }

  const { series, colors } = getData();

  function getTicks() {
    return [1, 21, 45, 69, 92];
  }

  return (
    <div>
      <div>
        <YearSlider
          sliderRange={[getFirstSeasonStartYear(), getLastSeasonEndYear()]}
          selectedFilterRange={selectedFilterRange}
          setSelectedFilterRange={setSelectedFilterRange}
        />
      </div>
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
    </div>
  );
};

export { HistoricalPositions };
