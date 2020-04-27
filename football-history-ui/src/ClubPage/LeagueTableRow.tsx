import React, { FunctionComponent, useState } from "react";
import { Row } from "./useLeagueTable";
import { Icon, Table } from "semantic-ui-react";
import { LeagueTableDrillDown } from "./LeagueTableDrillDown";
import { LeagueTableRowCell } from "./LeagueTableRowCell";

function getRowColor(row: Row, club: string) {
  let color = row.team === club ? "#CCCCCC" : null;
  switch (row.status) {
    case "Champions": {
      color = "#75B266";
      break;
    }
    case "Relegated": {
      color = "#B26694";
      break;
    }
    case "Promoted": {
      color = "#7FBFBF";
      break;
    }
    case "PlayOff Winner": {
      color = "#7FBFBF";
      break;
    }
    case "PlayOffs": {
      color = "#BFA67F";
      break;
    }
  }
  return color;
}

const LeagueTableRow: FunctionComponent<{
  row: Row;
  club: string;
  seasonStartYear: number;
  numRows: number;
  totalPlaces: number;
  promotionPlaces: number;
  playOffPlaces: number;
  relegationPlaces: number;
}> = ({
  row,
  club,
  seasonStartYear,
  numRows,
  totalPlaces,
  promotionPlaces,
  playOffPlaces,
  relegationPlaces,
}) => {
  const [showDrillDown, setShowDrillDown] = useState<boolean>(false);
  const bold = row.team === club;
  const color = getRowColor(row, club);

  function toggleDrillDown() {
    setShowDrillDown(!showDrillDown);
  }

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
        <LeagueTableRowCell bold={bold} color={color}>
          {row.points}
          {row.pointsDeducted > 0 ? " *" : ""}
        </LeagueTableRowCell>
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
