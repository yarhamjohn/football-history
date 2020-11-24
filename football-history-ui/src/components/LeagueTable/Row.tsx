import React, { FunctionComponent, useEffect, useState } from "react";
import { Row } from "../../hooks/useLeagueTable";
import { Icon, Table } from "semantic-ui-react";
import { LeagueTableDrillDown } from "./DrillDown";
import { LeagueTableRowCell } from "./Cell";
import { Color, getLeagueStatusColor } from "../../shared/functions";

function getRowColor(row: Row, club: string | undefined) {
    let color = getLeagueStatusColor(row.status);
    if (color === null && row.team === club) {
        color = Color.Grey;
    }
    return color;
}

const LeagueTableRow: FunctionComponent<{
    row: Row;
    club: string | undefined;
    seasonStartYear: number;
    numRows: number;
    totalPlaces: number;
    promotionPlaces: number;
    playOffPlaces: number;
    relegationPlaces: number;
    tier: number;
}> = ({
    row,
    club,
    seasonStartYear,
    numRows,
    totalPlaces,
    promotionPlaces,
    playOffPlaces,
    relegationPlaces,
    tier,
}) => {
    const [showDrillDown, setShowDrillDown] = useState<boolean>(false);
    const bold = row.team === club;
    const color = getRowColor(row, club);

    function toggleDrillDown() {
        setShowDrillDown(!showDrillDown);
    }

    useEffect(() => {
        setShowDrillDown(false);
    }, [club, seasonStartYear]);

    return (
        <>
            <Table.Row
                style={{
                    cursor: "pointer",
                }}
                onClick={() => toggleDrillDown()}
            >
                <LeagueTableRowCell bold={bold} color={color}>
                    {showDrillDown ? <Icon name="chevron down" /> : <Icon name="chevron right" />}
                </LeagueTableRowCell>
                <LeagueTableRowCell bold={bold} color={color}>
                    {row.position}
                </LeagueTableRowCell>
                <LeagueTableRowCell bold={bold} color={color}>
                    {row.team}
                </LeagueTableRowCell>
                <LeagueTableRowCell bold={bold} color={color}>
                    {row.played}
                </LeagueTableRowCell>
                <LeagueTableRowCell bold={bold} color={color}>
                    {row.won}
                </LeagueTableRowCell>
                <LeagueTableRowCell bold={bold} color={color}>
                    {row.drawn}
                </LeagueTableRowCell>
                <LeagueTableRowCell bold={bold} color={color}>
                    {row.lost}
                </LeagueTableRowCell>
                <LeagueTableRowCell bold={bold} color={color}>
                    {row.goalsFor}
                </LeagueTableRowCell>
                <LeagueTableRowCell bold={bold} color={color}>
                    {row.goalsAgainst}
                </LeagueTableRowCell>
                <LeagueTableRowCell bold={bold} color={color}>
                    {row.goalDifference}
                </LeagueTableRowCell>
                {seasonStartYear < 1976 ? (
                    <LeagueTableRowCell bold={bold} color={color}>
                        {Number(Math.round(parseFloat(row.goalAverage + "e4")) + "e-4")}
                    </LeagueTableRowCell>
                ) : null}
                <LeagueTableRowCell bold={bold} color={color}>
                    {row.points}
                    {row.pointsDeducted > 0 ? " *" : ""}
                </LeagueTableRowCell>
                {seasonStartYear === 2019 && (tier === 3 || tier === 4) ? (
                    <LeagueTableRowCell bold={bold} color={color}>
                        {Number(Math.round(parseFloat(row.pointsPerGame + "e2")) + "e-2")}
                    </LeagueTableRowCell>
                ) : null}
                <LeagueTableRowCell bold={bold} color={color}>
                    {row.status}
                </LeagueTableRowCell>
            </Table.Row>
            {showDrillDown ? (
                <LeagueTableDrillDown
                    club={row.team}
                    seasonStartYear={seasonStartYear}
                    numRows={numRows}
                    totalPlaces={totalPlaces}
                    promotionPlaces={promotionPlaces}
                    playOffPlaces={playOffPlaces}
                    relegationPlaces={relegationPlaces}
                />
            ) : null}
        </>
    );
};

export { LeagueTableRow };
