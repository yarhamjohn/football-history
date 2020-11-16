import React, { FunctionComponent, useState } from "react";
import {
    HistoricalPosition,
    HistoricalPositionsState,
    useHistoricalPositions,
} from "../../hooks/useHistoricalPositions";
import { Point, ResponsiveLine, Serie } from "@nivo/line";
import { Season as SeasonType } from "../../hooks/useSeasons";
import { YearSlider } from "../Filters/YearSlider";
import { getLeagueStatusColor } from "../../shared/functions";
import { Tooltip } from "./Tooltip";

const getDates = (start: number, end: number) =>
    Array.from({ length: end - start }, (v, k) => k + start);

function getPositions(
    allDates: number[],
    historicalPositionsState: {
        type: "HISTORICAL_POSITIONS_LOAD_SUCCEEDED";
        positions: HistoricalPosition[];
    }
) {
    return allDates.map((d) => {
        return {
            x: d,
            y: historicalPositionsState.positions.some((p) => p.seasonStartYear === d)
                ? historicalPositionsState.positions.filter((p) => p.seasonStartYear === d)[0]
                      .absolutePosition
                : null,
        };
    });
}

function getTierBottomBoundary(tier: 1 | 2 | 3, selectedFilterRange: number[]) {
    //TODO: this should calculate automatically based on League model info but I think the api needs changing to get this easily

    let result = [];
    if (tier === 1) {
        for (let i = selectedFilterRange[0]; i < selectedFilterRange[1]; i++) {
            if ((i >= 1976 && i <= 1986) || (i >= 1991 && i <= 1994)) {
                result.push({ x: i, y: 22.5 });
            } else if (i === 1987) {
                result.push({ x: i, y: 21.5 });
            } else {
                result.push({ x: i, y: 20.5 });
            }
        }
    } else if (tier === 2) {
        for (let i = selectedFilterRange[0]; i < selectedFilterRange[1]; i++) {
            if (i >= 1991 && i <= 1994) {
                result.push({ x: i, y: 46.5 });
            } else {
                result.push({ x: i, y: 44.5 });
            }
        }
    } else if (tier === 3) {
        for (let i = selectedFilterRange[0]; i < selectedFilterRange[1]; i++) {
            if (i >= 1991 && i <= 1994) {
                result.push({ x: i, y: 70.5 });
            } else {
                result.push({ x: i, y: 68.5 });
            }
        }
    }

    return result;
}

function getData(
    selectedFilterRange: number[],
    historicalPositionsState: HistoricalPositionsState
) {
    let series: Serie[] = [];
    let colors = ["black", "#75B266", "#BFA67F", "#B26694"];

    if (historicalPositionsState.type !== "HISTORICAL_POSITIONS_LOAD_SUCCEEDED") {
        return { series, colors };
    }

    const allDates = getDates(selectedFilterRange[0] - 1, selectedFilterRange[1] + 1);

    series = [
        {
            id: "positions",
            data: getPositions(allDates, historicalPositionsState),
        },
        {
            id: "tier1-tier2",
            data: getTierBottomBoundary(1, selectedFilterRange),
        },
        {
            id: "tier2-tier3",
            data: getTierBottomBoundary(2, selectedFilterRange),
        },
        {
            id: "tier3-tier4",
            data: getTierBottomBoundary(3, selectedFilterRange),
        },
    ];

    return { series, colors };
}

const HistoricalPositionsGraph: FunctionComponent<{
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
    const { series, colors } = getData(selectedFilterRange, historicalPositionsState);

    function getYAxisTicks() {
        return [1, 16, 31, 46, 61, 76, 92];
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
                    gridYValues={getYAxisTicks()}
                    enableSlices="x"
                    sliceTooltip={({ slice }) => {
                        return (
                            <Tooltip
                                points={slice.points}
                                seasons={seasons}
                                positions={
                                    historicalPositionsState.type ===
                                    "HISTORICAL_POSITIONS_LOAD_SUCCEEDED"
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

export { HistoricalPositionsGraph };
