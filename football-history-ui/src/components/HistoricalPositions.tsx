import React, { FunctionComponent, useState } from "react";
import { HistoricalPosition, useHistoricalPositions } from "../hooks/useHistoricalPositions";
import { Point, ResponsiveLine, Serie } from "@nivo/line";
import { Season as SeasonType } from "../hooks/useSeasons";
import { YearSlider } from "./Filters/YearSlider";
import { getLeagueStatusColor } from "../shared/functions";

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
                            ? historicalPositionsState.positions.filter(
                                  (p) => p.seasonStartYear === d
                              )[0].absolutePosition
                            : null,
                    };
                }),
            },
            {
                id: "tier1-tier2",
                data: [
                    { x: 1989, y: 20.5 },
                    { x: 1990, y: 20.5 },
                    { x: 1991, y: 22.5 },
                    { x: 1994, y: 22.5 },
                    { x: 1995, y: 20.5 },
                    { x: 2019, y: 20.5 },
                ],
            },
            {
                id: "tier2-tier3",
                data: [
                    { x: 1989, y: 44.5 },
                    { x: 1990, y: 44.5 },
                    { x: 1991, y: 46.5 },
                    { x: 1994, y: 46.5 },
                    { x: 1995, y: 44.5 },
                    { x: 2019, y: 44.5 },
                ],
            },
            {
                id: "tier3-tier4",
                data: [
                    { x: 1989, y: 68.5 },
                    { x: 1990, y: 68.5 },
                    { x: 1991, y: 70.5 },
                    { x: 1994, y: 70.5 },
                    { x: 1995, y: 68.5 },
                    { x: 2019, y: 68.5 },
                ],
            },
        ];

        return { series, colors };
    }

    const { series, colors } = getData();

    function getTicks() {
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
                    gridYValues={getTicks()}
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
        const historicalPositions = positions.filter((p) => p.seasonStartYear === season);
        const status = historicalPositions.length === 1 ? historicalPositions[0].status : "";
        return status;
    };

    const getStatusComponent = (absolutePosition: number, season: number) => {
        const status = getStatus(absolutePosition, season);

        const color = getLeagueStatusColor(status);
        if (color === null) {
            return null;
        }
        return <h3 style={{ color: color.toString() }}>{status}</h3>;
    };

    function isLeagueBoundaryPoint(position: number) {
        return position % 1 !== 0;
    }

    function getContent(point: Point) {
        let position = getPosition(point.data.yFormatted as number);
        return isLeagueBoundaryPoint(position) ? null : (
            <div
                key={point.id}
                style={{
                    color: point.serieColor,
                    padding: "12px 12px",
                    display: "flex",
                    flexDirection: "column",
                    boxShadow: `0px 0px 10px ${getLeagueStatusColor(
                        getStatus(point.data.yFormatted as number, point.data.xFormatted as number)
                    )} inset`,
                }}
            >
                {getStatusComponent(
                    point.data.yFormatted as number,
                    point.data.xFormatted as number
                )}
                <strong>
                    {getLeagueName(
                        point.data.yFormatted as number,
                        point.data.xFormatted as number
                    )}
                </strong>
                <span>
                    <strong>Position</strong>: {position}
                </span>
                <span>
                    <strong>Season</strong>: {point.data.xFormatted}-
                    {(point.data.xFormatted as number) + 1}
                </span>
            </div>
        );
    }

    return (
        <div
            style={{
                background: "white",
                border: "1px solid #ccc",
                borderRadius: "5px",
            }}
        >
            {points.map((point) => getContent(point))}
        </div>
    );
};

export { HistoricalPositions };
