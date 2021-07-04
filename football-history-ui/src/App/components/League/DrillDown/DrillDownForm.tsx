import React, { FunctionComponent } from "react";
import { Match } from "../../../shared/useFetchLeagueMatches";
import { useDrillDownForm } from "./useDrillDownForm";

const LeagueTableDrillDownForm: FunctionComponent<{
  matches: Match[];
  teamId: number;
}> = ({ matches, teamId }) => {
  const form = useDrillDownForm(matches, teamId);

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
};

export { LeagueTableDrillDownForm };
