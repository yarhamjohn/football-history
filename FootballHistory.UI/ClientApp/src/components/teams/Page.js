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
    const [selectedSeasons, setSelectedSeasons] = useState([1992, 2017]); //TODO: This should be dynamic somehow
    
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

    function fetchHistoricalPositions(existingHistoricalPositions, firstSeasonStartYear, lastSeasonStartYear) {
        fetch(`${baseUrl}/api/Team/GetHistoricalPositions?team=${selectedTeam}&firstSeasonStartYear=${firstSeasonStartYear}&lastSeasonStartYear=${lastSeasonStartYear}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to fetch')
                }

                return response.json()
            })
            .then(data => {
                setHistoricalPositions([...existingHistoricalPositions, ...data])
            })
            .catch(err => {
                console.log(err);
                setIsLoadingHistoricalPositions(false);
            })
    }
    
    useEffect(() => {
        if (selectedTeam !== "")
        {
            setIsLoadingHistoricalPositions(true);

            let seasonStartYears = historicalPositions.map(hp => parseInt(hp.season.substring(0, 4)));
            let currentFirstYear = Math.min(...seasonStartYears);
            let currentLastYear = Math.max(...seasonStartYears);
            let selectedFirstYear = Math.min(...selectedSeasons);
            let selectedLastYear = Math.max(...selectedSeasons);
            
            let allDataExists = selectedFirstYear >= currentFirstYear && selectedLastYear <= currentLastYear;
            if (allDataExists)
            {
                let newData = historicalPositions.filter(hp => {
                    let year = parseInt(hp.season.substring(0, 4));
                    return year >= selectedFirstYear && year <= selectedLastYear;
                });
                
                setHistoricalPositions(newData);
                return;
            }
            
            if (selectedFirstYear < currentFirstYear) {
                fetchHistoricalPositions(historicalPositions, selectedFirstYear, currentFirstYear - 1);
            } else {
                fetchHistoricalPositions(historicalPositions, currentLastYear + 1, selectedLastYear);
            }
        }
    }, [selectedSeasons]);

    useEffect(() => {
        setIsLoadingTeams(false);

        if (selectedTeam !== "")
        {
            setIsLoadingHistoricalPositions(true);

            fetchHistoricalPositions([], Math.min(...selectedSeasons), Math.max(...selectedSeasons));
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
                        allSeasons={_.range(1992, 2018, 1)} //TODO: This should be dynamic somehow
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
