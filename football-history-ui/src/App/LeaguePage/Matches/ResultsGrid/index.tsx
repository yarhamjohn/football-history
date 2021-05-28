import React, { FunctionComponent } from "react";
import { Match } from "../../../shared/useFetchLeagueMatches";
import { Table } from "semantic-ui-react";
import { ResultsGridRow } from "./Row";

const ResultsGrid: FunctionComponent<{ matches: Match[] }> = ({ matches }) => {
  const abbreviations = Array.from(new Set(matches.map((m) => m.homeTeam.abbreviation))).sort();

  return (
    <Table compact celled definition size="small" style={{ margin: 0 }}>
      <Table.Header fullWidth>
        <Table.Row>
          <Table.HeaderCell />
          {abbreviations.map((a) => (
            <Table.HeaderCell key={`HeaderCell: ${a}`}>{a}</Table.HeaderCell>
          ))}
        </Table.Row>
      </Table.Header>
      <Table.Body>
        {abbreviations.map((a) => (
          <ResultsGridRow
            key={a}
            matches={matches}
            abbreviations={abbreviations}
            teamAbbreviation={a}
          />
        ))}
      </Table.Body>
    </Table>
  );
};

export { ResultsGrid };
