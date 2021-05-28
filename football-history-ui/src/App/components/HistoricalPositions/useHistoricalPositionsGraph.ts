import { HistoricalPositionRange } from "./index";
import { HistoricalPosition } from "../../shared/useFetchHistoricalPositions";

const useHistoricalPositionsGraph = (
  positions: HistoricalPosition[],
  range: HistoricalPositionRange
) => {
  const getSeasonStartYears = (start: number, end: number) =>
    Array.from({ length: end - start }, (v, k) => k + start);

  const getPositionSeries = (positions: HistoricalPosition[], range: HistoricalPositionRange) =>
    getSeasonStartYears(range.startYear - 1, range.endYear + 2).map((d) => {
      return {
        x: d,
        y: positions.some((p) => p.seasonStartYear === d)
          ? positions.filter((p) => p.seasonStartYear === d)[0].team?.absolutePosition ?? null
          : null,
      };
    });

  function getBoundarySeries(level: "1" | "2" | "3", positions: HistoricalPosition[]) {
    //TODO: This method can't handle levels "3N" or "3S"

    return positions
      .sort((i, j) => i.seasonStartYear - j.seasonStartYear)
      .map((p) => {
        return {
          x: p.seasonStartYear,
          y:
            p.competitions
              .filter((c) => +c.level <= +level)
              .reduce((sum, current) => sum + current.rules.totalPlaces, 0) + 0.5,
        };
      });
  }

  const series = [
    {
      id: "positions",
      data: getPositionSeries(positions, range),
    },
    {
      id: "tier1-tier2",
      data: getBoundarySeries("1", positions),
    },
    {
      id: "tier2-tier3",
      data: getBoundarySeries("2", positions),
    },
    {
      id: "tier3-tier4",
      data: getBoundarySeries("3", positions),
    },
  ];

  const colors = ["black", "#75B266", "#BFA67F", "#B26694"];

  // TODO: This should be calculated dynamically
  const yValues = [1, 16, 31, 46, 61, 76, 92];

  return { series, colors, yValues };
};

export { useHistoricalPositionsGraph };
