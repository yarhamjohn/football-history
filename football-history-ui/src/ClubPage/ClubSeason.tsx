import React, { FunctionComponent, useEffect, useState } from "react";
import { Club } from "../hooks/useClubs";
import { useSeasons } from "../hooks/useSeasons";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { useTiers } from "../hooks/useTiers";
import { useLeagueTable } from "../hooks/useLeagueTable";
import { LeagueTable } from "../components/LeagueTable/LeagueTable";

const ClubSeason: FunctionComponent<{ selectedClub: string }> = ({ selectedClub }) => {
  const { tier, getTier } = useTiers();
  const { seasons } = useSeasons();
  const [selectedSeason, setSelectedSeason] = useState<number | undefined>(undefined);
  const { leagueTableState, getLeagueTableForTeam } = useLeagueTable();
  useEffect(() => {
    if (selectedSeason !== undefined && selectedClub !== undefined) {
      getLeagueTableForTeam(selectedClub, selectedSeason);
    }
  }, [selectedClub, selectedSeason]);

  useEffect(() => {
    if (seasons === undefined || selectedClub === undefined) {
      return;
    }

    setSelectedSeason(Math.max(...seasons.map((s) => s.startYear)));
  }, [selectedClub, seasons]);

  useEffect(() => {
    if (selectedSeason !== undefined) {
      getTier(selectedClub, selectedSeason);
    }
  }, [selectedClub, selectedSeason]);

  const getDivisionName = () => {
    if (seasons !== undefined) {
      const season = seasons.filter((s) => s.startYear === selectedSeason);

      if (season.length !== 1) {
        return;
      }

      const division = season[0].divisions.filter((d) => d.tier === tier);
      if (division.length !== 1) {
        return null;
      }

      return division[0].name;
    }
  };

  if (seasons === undefined) {
    return null;
  }

  return (
    <div style={{ display: "grid", gridGap: "1rem" }}>
      <SeasonFilter
        seasons={seasons}
        selectedSeason={selectedSeason}
        setSelectedSeason={(startYear) => setSelectedSeason(startYear)}
      />
      <h2 style={{ margin: 0 }}>{getDivisionName()}</h2>
      {leagueTableState.type !== "LEAGUE_TABLE_LOADED" ? null : (
        <div
          style={{
            display: "grid",
            gridTemplateColumns: "repeat(auto-fit, minmax(1100px, 1fr))",
            gridGap: "1rem",
          }}
        >
          <LeagueTable
            club={selectedClub}
            table={leagueTableState.leagueTable}
            seasonStartYear={selectedSeason}
          />
        </div>
      )}
    </div>
  );
};

export { ClubSeason };
