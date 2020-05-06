import React, { FunctionComponent, useEffect } from "react";
import { useLeagueMatches } from "./useLeagueMatches";
import { Table } from "semantic-ui-react";

const EmptyCell: FunctionComponent = () => {
  return <Table.Cell style={{ background: "rgba(0,0,0,.03)" }}></Table.Cell>;
};
const ResultsGrid: FunctionComponent<{ tier: number; seasonStartYear: number | undefined }> = ({
  tier,
  seasonStartYear,
}) => {
  const { allLeagueMatches, getAllLeagueMatches } = useLeagueMatches();

  useEffect(() => {
    if (seasonStartYear !== undefined) {
      getAllLeagueMatches(tier, seasonStartYear);
    }
  }, [tier, seasonStartYear]);

  if (allLeagueMatches == undefined) {
    return null;
  }

  const abbreviations = Array.from(
    new Set(allLeagueMatches.map((m) => m.homeTeamAbbreviation))
  ).sort();

  function getRow(teamAbbreviation: string) {
    if (allLeagueMatches == undefined) {
      return null;
    }

    const homeGames = allLeagueMatches
      .filter((m) => m.homeTeamAbbreviation === teamAbbreviation)
      .map((m) => {
        return {
          awayTeamAbbreviation: m.awayTeamAbbreviation,
          result: `${m.homeGoals}-${m.awayGoals}`,
        };
      });

    homeGames.push({ awayTeamAbbreviation: teamAbbreviation, result: "" });
    homeGames.sort((a, b) => {
      if (a.awayTeamAbbreviation > b.awayTeamAbbreviation) {
        return 1;
      }
      if (a.awayTeamAbbreviation < b.awayTeamAbbreviation) {
        return -1;
      }
      return 0;
    });

    const cells = homeGames.map((x) => {
      if (x.awayTeamAbbreviation === teamAbbreviation) {
        return <EmptyCell key={x.awayTeamAbbreviation} />;
      } else {
        return <Table.Cell key={x.awayTeamAbbreviation}>{x.result}</Table.Cell>;
      }
    });

    return (
      <Table.Row key={teamAbbreviation}>
        <Table.Cell>{teamAbbreviation}</Table.Cell>
        {cells}
      </Table.Row>
    );
  }

  return (
    <Table compact celled definition size="small" style={{ margin: 0 }}>
      <Table.Header fullWidth>
        <Table.Row>
          <Table.HeaderCell />
          {abbreviations.map((a) => (
            <Table.HeaderCell key={a}>{a}</Table.HeaderCell>
          ))}
        </Table.Row>
      </Table.Header>
      <Table.Body>{abbreviations.map((a) => getRow(a))}</Table.Body>
    </Table>
  );
};

export { ResultsGrid };
