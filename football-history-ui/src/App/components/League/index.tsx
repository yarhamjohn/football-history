import React, { FunctionComponent } from "react";
import { Loader } from "semantic-ui-react";
import { PointDeductionSummary } from "./PointDeductionSummary";
import { useFetchLeague } from "../../shared/useFetchLeague";
import { Competition } from "../../shared/useFetchCompetitions";
import { ErrorMessage } from "../ErrorMessage";
import { LeagueTable } from "./Table/Table";
import { Season } from "../../shared/seasonsSlice";
import { Team } from "../../shared/teamsSlice";

type FetchLeagueProps =
  | {
      season: Season;
      competition: Competition;
    }
  | {
      season: Season;
      team: Team;
    };

const League: FunctionComponent<{ props: FetchLeagueProps }> = ({ props }) => {
  const league = useFetchLeague(
    "team" in props
      ? { seasonId: props.season.id, teamId: props.team.id }
      : { competitionId: props.competition.id }
  );

  if (league.status === "UNLOADED") {
    return null;
  }

  if (league.status === "LOADING") {
    return <Loader />;
  }

  if (league.status === "LOAD_FAILED") {
    return <ErrorMessage header={"Something went wrong"} content={league.error} />;
  }

  return (
    <div>
      <LeagueTable
        league={league.data}
        selectedClub={"team" in props ? props.team : undefined}
        seasonStartYear={props.season.startYear}
      />
      <PointDeductionSummary leagueTable={league.data.table} />
    </div>
  );
};

export { League };
