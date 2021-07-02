import { Row } from "../../../shared/useFetchLeague";
import { Color, getLeagueStatusColor } from "../../../shared/functions";
import { Team } from "../../../shared/teamsSlice";

const useLeagueTableRow = (row: Row, selectedClub: Team | undefined) => {
  const isSelectedRow = row.team === selectedClub?.name;

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
