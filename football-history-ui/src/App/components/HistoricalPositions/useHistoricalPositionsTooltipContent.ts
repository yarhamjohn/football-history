import { getLeagueStatusColor } from "../../shared/functions";
import { Point } from "@nivo/line";
import { HistoricalPosition } from "../../shared/useFetchHistoricalPositions";

const useHistoricalPositionsTooltipContent = (point: Point, positions: HistoricalPosition[]) => {
  const seasonStartYear = point.data.xFormatted as number;
  const absolutePosition = point.data.yFormatted as number;

  const competitions = positions
    .flatMap((p) => p.competitions)
    .filter((c) => c.season.startYear === seasonStartYear)
    .sort((x, y) => (x.level > y.level ? 1 : -1));

  const boundaries = competitions.map(
    (comp, mapIdx, sortedComps) =>
      sortedComps
        .filter((_, filterIdx) => filterIdx < mapIdx)
        .map((filteredComp) => filteredComp.rules.totalPlaces)
        .reduce((sum, current) => sum + current, 0) + comp.rules.totalPlaces
  );
  boundaries.push(absolutePosition);
  boundaries.sort((x, y) => x - y);

  const posInArray = boundaries.indexOf(absolutePosition);
  const leaguePosition =
    posInArray === 0 ? absolutePosition : absolutePosition - boundaries[posInArray - 1];

  const historicalPosition = positions.filter((p) => p.seasonStartYear === seasonStartYear)[0];
  const leagueStatus = historicalPosition.team?.status ?? null;

  const leagueName = competitions[posInArray].name;
  const color = getLeagueStatusColor(leagueStatus);

  return { leaguePosition, leagueStatus, leagueName, color, absolutePosition, seasonStartYear };
};

export { useHistoricalPositionsTooltipContent };
