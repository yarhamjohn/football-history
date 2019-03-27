import React from "react";
import PropTypes from "prop-types";
import {ButtonToolbar} from "react-bootstrap";
import DropdownButton from "../shared/DropdownButton";
import YearSlider from "./YearSlider/YearSlider";

function Filter(props) {
    return (
        <div>
            <ButtonToolbar>
                <DropdownButton 
                    buttonLabel="Teams (A - C)" 
                    currentSelection={props.selectedTeam} 
                    dropdownItems={props.allTeams.filter(t => t.substring(0, 1) <= "C")} 
                    updateSelected={(t) => props.updateSelectedTeam(t)}
                    disableButton={props.disableButton} />
                <DropdownButton
                    buttonLabel="Teams (D - L)"
                    currentSelection={props.selectedTeam}
                    dropdownItems={props.allTeams.filter(t =>t.substring(0, 1) > "C" && t.substring(0, 1) <= "L")}
                    updateSelected={(t) => props.updateSelectedTeam(t)}
                    disableButton={props.disableButton} />
                <DropdownButton
                    buttonLabel="Teams (M - R)"
                    currentSelection={props.selectedTeam}
                    dropdownItems={props.allTeams.filter(t => t.substring(0, 1) > "L" && t.substring(0, 1) <= "R")}
                    updateSelected={(t) => props.updateSelectedTeam(t)}
                    disableButton={props.disableButton} />
                <DropdownButton
                    buttonLabel="Team (S - Z)"
                    currentSelection={props.selectedTeam}
                    dropdownItems={props.allTeams.filter(t => t.substring(0, 1) > "R")}
                    updateSelected={(t) => props.updateSelectedTeam(t)}
                    disableButton={props.disableButton} />
            </ButtonToolbar>
            <YearSlider 
                rangeStart={Math.min(...props.allSeasons)} 
                rangeEnd={Math.max(...props.allSeasons)} 
                selectedStart={Math.min(...props.selectedSeasons)} 
                selectedEnd={Math.max(...props.selectedSeasons)} 
                disableSlider={props.disableButton} 
                updateSelectedSeasons={(values) => props.updateSelectedSeasons(values)}/>
        </div>
    );
}

Filter.propTypes = {
    allTeams: PropTypes.arrayOf(PropTypes.string),
    selectedTeam: PropTypes.string,
    updateSelectedTeam: PropTypes.func,
    disableButton: PropTypes.bool,
    updateSelectedSeasons: PropTypes.func,
    selectedSeasons: PropTypes.arrayOf(PropTypes.number),
    allSeasons: PropTypes.arrayOf(PropTypes.number)
};

export default Filter;
