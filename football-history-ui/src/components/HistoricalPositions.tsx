import React, { FunctionComponent, useState } from "react";
import { HistoricalPosition, useHistoricalPositions } from "../hooks/useHistoricalPositions";
import { Point, ResponsiveLine, Serie } from "@nivo/line";
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
          enableSlices="x"
          sliceTooltip={({ slice }) => {
            return (
              <Tooltip
                points={slice.points}
                seasons={seasons}
                positions={
                  historicalPositionsState.type === "HISTORICAL_POSITIONS_LOAD_SUCCEEDED"
                    ? historicalPositionsState.positions
                    : []
                }
              />
            );
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

const Tooltip: FunctionComponent<{
  points: Point[];
  seasons: SeasonType[];
  positions: HistoricalPosition[];
}> = ({ points, seasons, positions }) => {
  const getPosition = (absolutePosition: number) => {
    if (absolutePosition <= 20) {
      return absolutePosition;
    } else if (absolutePosition <= 44) {
      return absolutePosition - 20;
    } else if (absolutePosition <= 68) {
      return absolutePosition - 44;
    } else {
      return absolutePosition - 68;
    }
  };

  const getLeagueName = (absolutePosition: number, season: number) => {
    let seasons1 = seasons.filter((s) => s.startYear === season);
    if (seasons1.length !== 1) {
      return "";
    }
    if (absolutePosition <= 20) {
      let divisions = seasons1[0].divisions.filter((d) => d.tier === 1);
      return divisions.length === 1 ? divisions[0].name : "";
    } else if (absolutePosition <= 44) {
      let divisions = seasons1[0].divisions.filter((d) => d.tier === 2);
      return divisions.length === 1 ? divisions[0].name : "";
    } else if (absolutePosition <= 68) {
      let divisions = seasons1[0].divisions.filter((d) => d.tier === 3);
      return divisions.length === 1 ? divisions[0].name : "";
    } else {
      let divisions = seasons1[0].divisions.filter((d) => d.tier === 4);
      return divisions.length === 1 ? divisions[0].name : "";
    }
  };

  const getStatus = (absolutePosition: number, season: number) => {
    let historicalPositions = positions.filter((p) => p.seasonStartYear === season);
    const status = historicalPositions.length === 1 ? historicalPositions[0].status : "";

    switch (status) {
      case "Champions":
        return <h3 style={{ color: "#75B266" }}>{status}</h3>;
      case "Promoted":
        return <h3 style={{ color: "#7FBFBF" }}>{status}</h3>;
      case "Relegated":
        return <h3 style={{ color: "#B26694" }}>{status}</h3>;
      case "PlayOffs":
        return <h3 style={{ color: "#BFA67F" }}>{status}</h3>;
      case "PlayOff Winner":
        return <h3 style={{ color: "#7FBFBF" }}>{status}</h3>;
      default:
        return null;
    }
  };

  return (
    <div
      style={{
        background: "white",
        padding: "9px 12px",
        border: "1px solid #ccc",
        borderRadius: "5px",
      }}
    >
      {points.map((point) => (
        <div
          key={point.id}
          style={{
            color: point.serieColor,
            padding: "3px 0",
            display: "flex",
            flexDirection: "column",
          }}
        >
          <>{getStatus(point.data.yFormatted as number, point.data.xFormatted as number)}</>
          <strong>
            {getLeagueName(point.data.yFormatted as number, point.data.xFormatted as number)}
          </strong>
          <span>
            <strong>Position</strong>: {getPosition(point.data.yFormatted as number)}
          </span>
          <span>
            <strong>Season</strong>: {point.data.xFormatted}-{(point.data.xFormatted as number) + 1}
          </span>
        </div>
      ))}
    </div>
  );
};

export { HistoricalPositions };
