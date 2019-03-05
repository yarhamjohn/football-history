import React, {useState, useEffect} from "react";
import {ButtonToolbar, DropdownButton, MenuItem} from "react-bootstrap";
import baseUrl from "../../api/LeagueSeasonApi";

function Filter(props) {
    const [allTeams, setAllTeams] = useState([]);
    const [selectedTeam, setSelectedTeam] = useState(props.selectedTeam);
    
    useEffect(() => {
        fetch(`${baseUrl}/api/Team/GetTeamFilters`)
            .then(response => response.json())
            .then(data => { 
                setAllTeams(data);
                
                let sorted = data.sort((a, b) => a > b);
                let team = sorted.length > 0 ? sorted[0] : "";
                updateSelectedTeam(team);
            }); 
    }, []);
    
    const updateSelectedTeam = (team) => {
        setSelectedTeam(team);
        props.updateSelectedTeam(team);
    };

    return (
        <ButtonToolbar className='filter-buttons'>
            <DropdownButton title="Teams" id="TeamSelect">
                {
                    allTeams.map(t =>
                        <MenuItem key={t} eventKey={t}
                                  className={t === selectedTeam ? "active" : ""}
                                  onSelect={(t) => updateSelectedTeam(t)}
                        >
                            {t}
                        </MenuItem>)
                }
            </DropdownButton>
        </ButtonToolbar>
    );
}

export default Filter;
