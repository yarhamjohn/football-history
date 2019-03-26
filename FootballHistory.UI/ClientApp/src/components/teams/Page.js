import React, {useEffect, useState} from 'react';
import Filter from "./Filter";
import _ from "underscore";
import HistoricalPositions from "./HistoricalPositions";
import baseUrl from "../../api/LeagueSeasonApi";
import './Page.css';
import {Spinner} from "react-bootstrap";

function Page() {
    const [selectedTeam, setSelectedTeam] = useState("");
    const [allTeams, setAllTeams] = useState([]);
    const [isLoadingTeams, setIsLoadingTeams] = useState(true);
    const [historicalPositions, setHistoricalPositions] = useState([]);
    const [isLoadingHistoricalPositions, setIsLoadingHistoricalPositions] = useState(true);
    const [selectedSeasons, setSelectedSeasons] = useState([]);
    
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

    useEffect(() => {
        setIsLoadingTeams(false);

        if (selectedTeam !== "")
        {
            setIsLoadingHistoricalPositions(true);

            fetch(`${baseUrl}/api/Team/GetHistoricalPositions?team=${selectedTeam}&firstSeasonStartYear=${1992}&lastSeasonStartYear=${2017}`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Failed to fetch')
                    }

                    return response.json()
                })
                .then(data => {
                    setHistoricalPositions(data)
                })
                .catch(err => {
                    console.log(err);
                    setIsLoadingHistoricalPositions(false);
                })
        }
    }, [selectedTeam]);
    
    useEffect(() => {
        if (historicalPositions.length > 0)
        {
            setIsLoadingHistoricalPositions(false);
        }
    }, [historicalPositions]);
    
    return (
        isLoadingTeams
            ? <Spinner animation='border' variant='info' />
            : <div>
                <h1 className='header'>{selectedTeam}</h1>
                <div className='filter-container'>
                    <Filter 
                        updateSelectedTeam={(team) => setSelectedTeam(team)} 
                        selectedTeam={selectedTeam}
                        allTeams={allTeams}
                        disableButton={isLoadingHistoricalPositions}
                        updateSelectedSeasons={(values) => setSelectedSeasons(values)}
                        selectedSeasons={selectedSeasons}
                        allSeasons={_.range(1992, 2018, 1)}
                    />
                </div>
                <div className='graph-container'>
                {
                    isLoadingHistoricalPositions
                        ? <Spinner animation='border' variant='info'/>
                        : <HistoricalPositions historicalPositions={historicalPositions}/>
                }
                </div>
            </div>
      );
}

export default Page;
