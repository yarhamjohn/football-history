import { Row } from "../../../shared/useFetchLeague";
import { Color, getLeagueStatusColor } from "../../../shared/functions";
import { Team } from "../../../shared/teamsSlice";

const useLeagueTableRow = (row: Row, selectedTeam: Team | undefined) => {
  const isSelectedRow = row.team === selectedTeam?.name;

  let color = getLeagueStatusColor(row.status);
  if (color === null && isSelectedRow) {
    color = Color.Grey;
  }

  return {
    bold: isSelectedRow,
    color,
  };
};

export { useLeagueTableRow };
