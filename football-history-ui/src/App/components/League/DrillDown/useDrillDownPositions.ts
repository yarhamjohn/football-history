import { LeaguePosition } from "../../../shared/useFetchLeaguePositions";
import { CompetitionRules } from "../../../shared/useFetchCompetitions";

const useDrillDownPositions = (
  positions: LeaguePosition[],
  rules: CompetitionRules,
  numRows: number
) => {
  const getMinDate = () =>
    new Date(
      Math.min.apply(
        null,
        positions.map((p) => new Date(p.date).valueOf())
      )
    );

  const getMaxDate = () =>
    new Date(
      Math.max.apply(
        null,
        positions.map((p) => new Date(p.date).valueOf())
      )
    );

  const getPositionData = () =>
    positions
      .map((p) => {
        return { x: new Date(p.date), y: p.position };
      })
      .sort();

  const getData = () => {
    let data = [
      {
        id: "positions",
        data: getPositionData(),
      },
    ];
    let colors = ["black"];

    if (rules.promotionPlaces > 0) {
      data.push({
        id: "promotion",
        data: [
          { x: getMinDate(), y: rules.promotionPlaces },
          { x: getMaxDate(), y: rules.promotionPlaces },
        ],
      });

      colors.push("#7FBFBF");
    }

    if (rules.playOffPlaces > 0) {
      data.push({
        id: "playOffs",
        data: [
          { x: getMinDate(), y: rules.promotionPlaces + rules.playOffPlaces },
          { x: getMaxDate(), y: rules.promotionPlaces + rules.playOffPlaces },
        ],
      });

      colors.push("#BFA67F");
    }

    if (rules.relegationPlaces > 0) {
      data.push({
        id: "relegation",
        data: [
          { x: getMinDate(), y: rules.totalPlaces - rules.relegationPlaces + 1 },
          { x: getMaxDate(), y: rules.totalPlaces - rules.relegationPlaces + 1 },
        ],
      });

      colors.push("#B26694");
    }

    return { data, colors };
  };

  const gridYValues = numRows === 20 ? [1, 5, 10, 15, 20] : [1, 6, 12, 18, 24];
  const gridXValues = positions.map((p) => new Date(p.date)).filter((d) => d.getUTCDate() === 1);

  return { ...getData(), gridYValues, gridXValues };
};

export { useDrillDownPositions };
