import { Row } from "../../../shared/useFetchLeague";
import { Team } from "../../../shared/useFetchClubs";
import { Color, getLeagueStatusColor } from "../../../shared/functions";

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
