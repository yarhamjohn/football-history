import React, { FunctionComponent, useEffect } from "react";
import { Card } from "semantic-ui-react";
import { useLeaguePositions } from "./useLeaguePositions";
import { ResponsiveLine } from "@nivo/line";

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
  const { leaguePositions, getLeaguePositions } = useLeaguePositions();

  useEffect(() => {
    getLeaguePositions(club, seasonStartYear);
  }, [club, seasonStartYear]);

  function getTicks(numRows: number) {
    if (numRows === 20) {
      return [1, 5, 10, 15, 20];
    }

    return [1, 6, 12, 18, 24];
  }

  function getDates() {
    if (leaguePositions) {
      let r = leaguePositions.map((p) => p.date).filter((d) => new Date(d).getUTCDate() === 1);
      console.log(r);
      return r;
    }

    return [];
  }

  function getMinDate() {
    return leaguePositions
      ? new Date(
          Math.min.apply(
            null,
            leaguePositions.map((p) => new Date(p.date).valueOf())
          )
        )
      : new Date();
  }

  function getMaxDate() {
    return leaguePositions
      ? new Date(
          Math.max.apply(
            null,
            leaguePositions.map((p) => new Date(p.date).valueOf())
          )
        )
      : new Date();
  }

  function getPositionData() {
    return leaguePositions
      ? leaguePositions
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

    if (promotionPlaces > 0) {
      data.push({
        id: "promotion",
        data: [
          { x: getMinDate(), y: promotionPlaces },
          { x: getMaxDate(), y: promotionPlaces },
        ],
      });
    }
    if (playOffPlaces > 0) {
      data.push({
        id: "playOffs",
        data: [
          { x: getMinDate(), y: promotionPlaces + playOffPlaces },
          { x: getMaxDate(), y: promotionPlaces + playOffPlaces },
        ],
      });
    }
    if (relegationPlaces > 0) {
      data.push({
        id: "relegation",
        data: [
          { x: getMinDate(), y: totalPlaces - relegationPlaces + 1 },
          { x: getMaxDate(), y: totalPlaces - relegationPlaces + 1 },
        ],
      });
    }

    return data;
  }

  const data = getData();

  return (
    <tr>
      <td colSpan={12}>
        <Card fluid>
          <Card.Content className="drilldown-card-body">
            <div className="drilldown-card-content" style={{ height: "200px" }}>
              <ResponsiveLine
                data={data}
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
