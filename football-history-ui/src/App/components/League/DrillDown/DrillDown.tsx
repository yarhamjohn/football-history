import React, { FunctionComponent } from "react";
import { Card, Loader } from "semantic-ui-react";
import { CompetitionRules } from "../../../shared/useFetchCompetitions";
import { ErrorMessage } from "../../ErrorMessage";
import { LeagueTableDrillDownForm } from "./DrillDownForm";
import { LeagueTableDrillDownPositions } from "./DrillDownPositions";
import { useFetchDrillDown } from "../../../shared/useFetchDrillDown";

const LeagueTableDrillDown: FunctionComponent<{
  teamId: number;
  competitionId: number;
  numRows: number;
  rules: CompetitionRules;
}> = ({ teamId, competitionId, numRows, rules }) => {
  const { leaguePositions, leagueMatches } = useFetchDrillDown(competitionId, teamId);

  return (
    <tr>
      <td colSpan={14}>
        <Card fluid>
          <Card.Content>
            {leagueMatches.status === "LOAD_SUCCESSFUL" && (
              <LeagueTableDrillDownForm matches={leagueMatches.data} clubId={teamId} />
            )}
            <div style={{ height: "200px", position: "relative" }}>
              {leaguePositions.status === "UNLOADED" ? null : leaguePositions.status ===
                "LOADING" ? (
                <Loader active style={{ position: "absolute", top: "50%", left: "50%" }} />
              ) : leaguePositions.status === "LOAD_FAILED" ? (
                <ErrorMessage header={"Something went wrong"} content={leaguePositions.error} />
              ) : (
                <LeagueTableDrillDownPositions
                  positions={leaguePositions.data}
                  rules={rules}
                  numRows={numRows}
                />
              )}
            </div>
          </Card.Content>
        </Card>
      </td>
    </tr>
  );
};

export { LeagueTableDrillDown };
