import React, { useState, useEffect } from 'react';
import Filter from "./Filter";
import baseUrl from "../../api/LeagueSeasonApi";

function Page() {
    const [selectedTeam, setSelectedTeam] = useState("");
    const [allTeams, setAllTeams] = useState([]);

    useEffect(() => {
        fetch(`${baseUrl}/api/Team/GetTeamFilters`)
            .then(response => response.json())
            .then(data => {
                setAllTeams(data);
    
                let sorted = data.sort((a, b) => a > b);
                let team = sorted.length > 0 ? sorted[0] : "";
                setSelectedTeam(team);
            });
    }, []);
    
  return (
      <div>
        <h1>{selectedTeam}</h1>
          <Filter updateSelectedTeam={(team) => setSelectedTeam(team)} selectedTeam={selectedTeam} allTeams={allTeams}/>
      </div>
  );
}

export default Page;
