import React, { FunctionComponent } from "react";
import { Table } from "semantic-ui-react";

const LeagueTableRowCell: FunctionComponent<{
  bold: boolean;
  color: string | null;
}> = ({ children, bold, color }) => {
  const style = bold ? { fontWeight: "bold", backgroundColor: color } : { backgroundColor: color };

  return <Table.Cell style={style}>{children}</Table.Cell>;
};

export { LeagueTableRowCell };
