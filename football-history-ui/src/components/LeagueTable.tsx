import React, { FunctionComponent, useEffect } from "react";
import { useLeagueTable } from "../ClubPage/useLeagueTable";
import { Table } from "semantic-ui-react";

const LeagueTable: FunctionComponent<{ club: string; seasonStartYear: number | undefined }> = ({
  club,
  seasonStartYear,
}) => {
  const { leagueTable, getLeagueTable } = useLeagueTable();

  useEffect(() => {
    if (seasonStartYear !== undefined) {
      getLeagueTable(club, seasonStartYear);
    }
  }, [club, seasonStartYear]);

  return (
    <>
      {leagueTable === undefined || leagueTable.table === null ? (
        <p>
          The selected club was not in the Football League or Premier League in the season
          specified.
        </p>
      ) : (
        <>
          <h1>{leagueTable.startYear}</h1>
          <Table>
            <Table.Header>
              <Table.Row>
                <Table.HeaderCell>Position</Table.HeaderCell>
                <Table.HeaderCell>Team</Table.HeaderCell>
                <Table.HeaderCell>Played</Table.HeaderCell>
                <Table.HeaderCell>Won</Table.HeaderCell>
                <Table.HeaderCell>Drawn</Table.HeaderCell>
                <Table.HeaderCell>Lost</Table.HeaderCell>
                <Table.HeaderCell>GF</Table.HeaderCell>
                <Table.HeaderCell>GA</Table.HeaderCell>
                <Table.HeaderCell>Diff</Table.HeaderCell>
                <Table.HeaderCell>Points</Table.HeaderCell>
                <Table.HeaderCell>Status</Table.HeaderCell>
              </Table.Row>
            </Table.Header>
            <Table.Body>
              {leagueTable &&
                leagueTable.table.map((r) => (
                  <Table.Row key={r.position}>
                    <Table.HeaderCell>{r.position}</Table.HeaderCell>
                    <Table.HeaderCell>{r.team}</Table.HeaderCell>
                    <Table.HeaderCell>{r.played}</Table.HeaderCell>
                    <Table.HeaderCell>{r.won}</Table.HeaderCell>
                    <Table.HeaderCell>{r.drawn}</Table.HeaderCell>
                    <Table.HeaderCell>{r.lost}</Table.HeaderCell>
                    <Table.HeaderCell>{r.goalsFor}</Table.HeaderCell>
                    <Table.HeaderCell>{r.goalsAgainst}</Table.HeaderCell>
                    <Table.HeaderCell>{r.goalDifference}</Table.HeaderCell>
                    <Table.HeaderCell>{r.points}</Table.HeaderCell>
                    <Table.HeaderCell>{r.status}</Table.HeaderCell>
                  </Table.Row>
                ))}
            </Table.Body>
          </Table>
        </>
      )}
    </>
  );
};

export { LeagueTable };
