import React, {useState, useEffect} from "react";
import {ButtonToolbar, DropdownButton, MenuItem} from "react-bootstrap";

function Filter(props) {
    const [allTeams, setAllTeams] = useState(props.allTeams);
    const [selectedTeam, setSelectedTeam] = useState(props.selectedTeam);
    
    useEffect(() => {
        setAllTeams(props.allTeams);
        setSelectedTeam(props.selectedTeam);
    });
    
    const updateSelectedTeam = (team) => {
        setSelectedTeam(team);
        props.updateSelectedTeam(team);
    };

    return (
        <div style={{marginBottom:'1rem'}}>
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
        </div>
    );
}

export default Filter;
