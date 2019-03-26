import React from "react";
import PropTypes from "prop-types";
import {ButtonToolbar} from "react-bootstrap";
import DropdownButton from "../shared/DropdownButton";

function Filter(props) {
    return (
        <ButtonToolbar>
            <DropdownButton 
                buttonLabel="Team" 
                currentSelection={props.selectedTeam} 
                dropdownItems={props.allTeams} 
                updateSelected={(t) => props.updateSelectedTeam(t)}
                disableButton={props.disableButton} />
        </ButtonToolbar>
    );
}

Filter.propTypes = {
    allTeams: PropTypes.arrayOf(PropTypes.string),
    selectedTeam: PropTypes.string,
    updateSelectedTeam: PropTypes.func,
    disableButton: PropTypes.bool
};

export default Filter;
