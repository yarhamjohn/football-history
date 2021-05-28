import React, { FunctionComponent } from "react";
import { Row } from "../../shared/useFetchLeague";

const PointDeductionSummary: FunctionComponent<{ leagueTable: Row[] }> = ({ leagueTable }) => {
  const pointDeductionRows = leagueTable.filter((r) => r.pointsDeducted !== 0);
  return (
    <div>
      {pointDeductionRows.map((r) => (
        <p key={r.position}>
          * {r.team}: {r.pointsDeducted} point{r.pointsDeducted === 1 ? "" : "s"} deducted -{" "}
          {r.pointsDeductionReason}
        </p>
      ))}
    </div>
  );
};

export { PointDeductionSummary };
