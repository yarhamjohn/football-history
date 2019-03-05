import React, {useState} from "react";
import {ButtonToolbar, DropdownButton, MenuItem} from "react-bootstrap";

function Filter(props) {
    const [allTeams, setAllTeams] = useState(["Arsenal", "Norwich"]);
    const [selectedTeam, setSelectedTeam] = useState(props.selectedTeam);
    
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
