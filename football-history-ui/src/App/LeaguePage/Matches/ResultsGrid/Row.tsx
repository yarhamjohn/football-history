import React, { FunctionComponent } from "react";
import { Match } from "../../../shared/useFetchLeagueMatches";
import { Table } from "semantic-ui-react";
import { EmptyCell, PopulatedCell } from "./Cell";

const ResultsGridRow: FunctionComponent<{
  matches: Match[];
  abbreviations: string[];
  teamAbbreviation: string;
}> = ({ matches, abbreviations, teamAbbreviation }) => {
  const homeGames = matches.filter((m) => m.homeTeam.abbreviation === teamAbbreviation);
  const missingAbbreviations = abbreviations.filter(
    (x) => !homeGames.map((m) => m.awayTeam.abbreviation).includes(x)
  );

  // Add a fake game to ensure the layout is right
  for (let abb of missingAbbreviations) {
    homeGames.push({
      homeTeam: { abbreviation: abb },
      awayTeam: { abbreviation: abb },
    } as Match);
  }

  homeGames.sort((a, b) => {
    if (a.awayTeam.abbreviation > b.awayTeam.abbreviation) {
      return 1;
    }
    if (a.awayTeam.abbreviation < b.awayTeam.abbreviation) {
      return -1;
    }
    return 0;
  });

  return (
    <Table.Row key={`Row: ${teamAbbreviation}`}>
      <Table.Cell>{teamAbbreviation}</Table.Cell>
      {homeGames.map((x) => {
        if (missingAbbreviations.includes(x.awayTeam.abbreviation)) {
          return <EmptyCell key={`Cell: ${x.homeTeam.abbreviation}-${x.awayTeam.abbreviation}`} />;
        } else {
          return (
            <PopulatedCell
              key={`Cell: ${x.homeTeam.abbreviation}-${x.awayTeam.abbreviation}`}
              match={x}
            />
          );
        }
      })}
    </Table.Row>
  );
};

export { ResultsGridRow };
