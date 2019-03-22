import React, {useState, useEffect} from "react";
import {ButtonToolbar, DropdownButton, DropdownItem} from "react-bootstrap";

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
                            <DropdownItem key={t} eventKey={t}
                                      className={t === selectedTeam ? "active" : ""}
                                      onSelect={(t) => updateSelectedTeam(t)}
                            >
                                {t}
                            </DropdownItem>)
                    }
                </DropdownButton>
            </ButtonToolbar>
        </div>
    );
}

export default Filter;
