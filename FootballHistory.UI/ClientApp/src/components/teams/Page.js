import React, {useEffect, useState} from 'react';
import Filter from "./Filter";
import HistoricalPositions from "./HistoricalPositions";
import baseUrl from "../../api/LeagueSeasonApi";
import './Page.css';
import {Spinner} from "react-bootstrap";

function Page() {
    const [selectedTeam, setSelectedTeam] = useState("");
    const [allTeams, setAllTeams] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    
    function firstTeamAlphabetically(data) {
        let sorted = data.sort();
        return sorted.length > 0 ? sorted[0] : "";
    }
    
    useEffect(() => {
        setIsLoading(false);    
    }, [selectedTeam]);

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
      isLoading
          ? <Spinner animation='border' variant='info' />
          : <div>
              <h1 className='header'>{selectedTeam}</h1>
              <div className='filter-container'>
                  <Filter updateSelectedTeam={(team) => setSelectedTeam(team)} selectedTeam={selectedTeam}
                          allTeams={allTeams}/>
              </div>
              <div className='graph-container'>
                  <HistoricalPositions selectedTeam={selectedTeam}/>
              </div>
            </div>
      );
}

export default Page;
