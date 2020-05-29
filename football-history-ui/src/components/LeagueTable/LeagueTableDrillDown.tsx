import React, { FunctionComponent } from "react";
import { Card } from "semantic-ui-react";
import { useLeaguePositions } from "../../hooks/useLeaguePositions";
import { ResponsiveLine } from "@nivo/line";
import { LeagueMatch, useLeagueMatches } from "../../hooks/useLeagueMatches";

const LeagueTableDrillDown: FunctionComponent<{
  club: string;
  seasonStartYear: number;
  numRows: number;
  totalPlaces: number;
  promotionPlaces: number;
  playOffPlaces: number;
  relegationPlaces: number;
}> = ({
  club,
  seasonStartYear,
  numRows,
  totalPlaces,
  promotionPlaces,
  playOffPlaces,
  relegationPlaces,
}) => {
  const { leaguePositionsState } = useLeaguePositions(club, seasonStartYear);
  const { leagueMatchesState } = useLeagueMatches(seasonStartYear, club);

  function getTicks(numRows: number) {
    if (numRows === 20) {
      return [1, 5, 10, 15, 20];
    }

    return [1, 6, 12, 18, 24];
  }

  function getDates() {
    return leaguePositionsState.type === "LEAGUE_POSITIONS_LOAD_SUCCEEDED"
      ? leaguePositionsState.positions
          .map((p) => new Date(p.date))
          .filter((d) => d.getUTCDate() === 1)
      : [];
  }

  function getMinDate() {
    return leaguePositionsState.type === "LEAGUE_POSITIONS_LOAD_SUCCEEDED"
      ? new Date(
          Math.min.apply(
            null,
            leaguePositionsState.positions.map((p) => new Date(p.date).valueOf())
          )
        )
      : new Date();
  }

  function getMaxDate() {
    return leaguePositionsState.type === "LEAGUE_POSITIONS_LOAD_SUCCEEDED"
      ? new Date(
          Math.max.apply(
            null,
            leaguePositionsState.positions.map((p) => new Date(p.date).valueOf())
          )
        )
      : new Date();
  }

  function getPositionData() {
    return leaguePositionsState.type === "LEAGUE_POSITIONS_LOAD_SUCCEEDED"
      ? leaguePositionsState.positions
          .map((p) => {
            return { x: new Date(p.date), y: p.position };
          })
          .sort()
      : [];
  }

  function getData() {
    let data = [
      {
        id: "positions",
        data: getPositionData(),
      },
    ];
    let colors = ["black"];

    if (promotionPlaces > 0) {
      data.push({
        id: "promotion",
        data: [
          { x: getMinDate(), y: promotionPlaces },
          { x: getMaxDate(), y: promotionPlaces },
        ],
      });

      colors.push("#7FBFBF");
    }
    if (playOffPlaces > 0) {
      data.push({
        id: "playOffs",
        data: [
          { x: getMinDate(), y: promotionPlaces + playOffPlaces },
          { x: getMaxDate(), y: promotionPlaces + playOffPlaces },
        ],
      });

      colors.push("#BFA67F");
    }
    if (relegationPlaces > 0) {
      data.push({
        id: "relegation",
        data: [
          { x: getMinDate(), y: totalPlaces - relegationPlaces + 1 },
          { x: getMaxDate(), y: totalPlaces - relegationPlaces + 1 },
        ],
      });

      colors.push("#B26694");
    }

    return { data, colors };
  }

  function GetForm(matches: LeagueMatch[]) {
    let form = matches
      .sort((a, b) => b.date.valueOf() - a.date.valueOf())
      .map((m) =>
        m.homeTeam === club && m.homeGoals > m.awayGoals
          ? "W"
          : m.awayTeam === club && m.awayGoals > m.homeGoals
          ? "W"
          : m.homeGoals === m.awayGoals
          ? "D"
          : "L"
      );
    return (
      <div style={{ display: "flex", justifyContent: "space-evenly" }}>
        {form.map((f, i) => (
          <span
            key={i}
            style={{
              fontWeight: "bold",
              color: f === "W" ? "#75B266" : f === "L" ? "#B26694" : "black",
            }}
          >
            {f}
          </span>
        ))}
      </div>
    );
  }

  const { data, colors } = getData();

  return (
    <tr>
      <td colSpan={12}>
        <Card fluid>
          <Card.Content>
            {leagueMatchesState.type === "LEAGUE_MATCHES_LOAD_SUCCEEDED" && (
              <div>{GetForm(leagueMatchesState.matches)}</div>
            )}
            <div style={{ height: "200px" }}>
              <ResponsiveLine
                data={data}
                colors={colors}
                margin={{ left: 25, bottom: 10, top: 10 }}
                yScale={{ type: "linear", min: 1, max: numRows, reverse: true }}
                enablePoints={false}
                gridYValues={getTicks(numRows)}
                gridXValues={getDates()}
                axisBottom={null}
              />
            </div>
          </Card.Content>
        </Card>
      </td>
    </tr>
  );
};

export { LeagueTableDrillDown };
