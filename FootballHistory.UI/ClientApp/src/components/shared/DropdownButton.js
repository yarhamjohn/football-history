import React from "react";
import PropTypes from "prop-types";
import {Dropdown} from "react-bootstrap";
import './DropdownButton.css';

function DropdownButton(props) {
    return (
        <Dropdown className='filter-button'>
            <Dropdown.Toggle variant='outline-primary'>{props.buttonLabel}</Dropdown.Toggle>
            <Dropdown.Menu flip={false}>
                {
                    props.dropdownItems.map(i =>
                        <Dropdown.Item key={i} eventKey={i}
                                      className={i === props.currentSelection ? "active" : ""}
                                      onSelect={(i) => props.updateSelected(i)}
                        >
                            {i}
                        </Dropdown.Item>)
                }
            </Dropdown.Menu>
        </Dropdown>        
    );
}

DropdownButton.propTypes = {
    buttonLabel: PropTypes.string,
    dropdownItems: PropTypes.arrayOf(PropTypes.string),
    currentSelection: PropTypes.string,
    updateSelected: PropTypes.func
};

export default DropdownButton;
