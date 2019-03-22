import React, {useEffect, useState} from 'react';
import { Spinner } from 'react-bootstrap';
import Filter from "./Filter";
import HistoricalPositions from "./HistoricalPositions";
import baseUrl from "../../api/LeagueSeasonApi";

function Page() {
    const [selectedTeam, setSelectedTeam] = useState("");
    const [allTeams, setAllTeams] = useState([]);

    function firstTeamAlphabetically(data) {
        let sorted = data.sort();
        return sorted.length > 0 ? sorted[0] : "";
    }

    useEffect(() => {
        fetch(`${baseUrl}/api/Team/GetTeamFilters`)
            .then(response => response.json())
            .then(data => {
                setAllTeams(data);
                
                let team = firstTeamAlphabetically(data);
                setSelectedTeam(team);
            });
    }, []);
    
  return (
      <div>
          <h1>{selectedTeam}</h1>
          <Filter updateSelectedTeam={(team) => setSelectedTeam(team)} selectedTeam={selectedTeam} allTeams={allTeams}/>
          <HistoricalPositions selectedTeam={selectedTeam} />
      </div>
  );
}

export default Page;
