import React, { FunctionComponent } from "react";
import { Match } from "../../../shared/useFetchLeagueMatches";
import { Table } from "semantic-ui-react";
import moment from "moment";

const EmptyCell: FunctionComponent = () => {
  return <Table.Cell style={{ background: "rgba(0,0,0,.03)" }}></Table.Cell>;
};

const PopulatedCell: FunctionComponent<{ match: Match }> = ({ match }) => {
  const GetColor = (homeGoals: number, awayGoals: number) => {
    if (homeGoals > awayGoals) {
      return "#75B266";
    }

    if (homeGoals < awayGoals) {
      return "#B26694";
    }

    return "#BFA67F";
  };

  return (
    <Table.Cell
      key={`Cell: ${match.homeTeam.abbreviation}-${match.awayTeam.abbreviation}`}
      style={{
        cursor: "context-menu",
        backgroundColor: GetColor(match.homeTeam.goals, match.awayTeam.goals),
      }}
      title={`${moment(match.matchDate).format("ddd, Do MMM YYYY")}: ${match.homeTeam.name} ${
        match.homeTeam.goals
      } - ${match.awayTeam.goals} ${match.awayTeam.name}`}
    >
      {`${match.homeTeam.goals}-${match.awayTeam.goals}`}
    </Table.Cell>
  );
};

export { PopulatedCell, EmptyCell };
