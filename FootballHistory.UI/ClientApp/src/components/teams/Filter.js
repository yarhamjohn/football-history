import React, {useState, useEffect} from "react";
import {Dropdown, ButtonToolbar} from "react-bootstrap";
import './Filter.css';

function Filter(props) {
    const [allTeams, setAllTeams] = useState(props.allTeams);
    const [selectedTeam, setSelectedTeam] = useState(props.selectedTeam);
    
    useEffect(() => {
        setAllTeams(props.allTeams);
    }, [props.allTeams]);

    useEffect(() => {
        setSelectedTeam(props.selectedTeam);
    }, [props.selectedTeam]);
    
    const updateSelectedTeam = (team) => {
        setSelectedTeam(team);
        props.updateSelectedTeam(team);
    };

    return (
        <ButtonToolbar>
            <Dropdown className='filter-button'>
                <Dropdown.Toggle variant='outline-primary'>Team</Dropdown.Toggle>
                <Dropdown.Menu flip={false} style={{marginTop:5}}>
                    {
                        allTeams.map(t =>
                            <Dropdown.Item key={t} eventKey={t}
                                          className={t === selectedTeam ? "active" : ""}
                                          onSelect={(t) => updateSelectedTeam(t)}
                            >
                                {t}
                            </Dropdown.Item>)
                    }
                </Dropdown.Menu>
            </Dropdown>        
        </ButtonToolbar>
    );
}

export default Filter;
