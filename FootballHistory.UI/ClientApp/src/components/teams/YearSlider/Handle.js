import React from "react";
import PropTypes from "prop-types";
import "./Handle.css";

function Handle(props) {
    return (
        <div
            role="slider"
            aria-valuemin={Math.min(...props.domain)}
            aria-valuemax={Math.max(...props.domain)}
            aria-valuenow={props.handle.value}
            className='handle-marker'
            style={{ left: `${props.handle.percent}%` }}
            {...props.getHandleProps(props.handle.id)}
        />
    )
}

Handle.propTypes = {
    domain: PropTypes.arrayOf(PropTypes.number),
    handle: PropTypes.shape({
        value: PropTypes.number,
        percent: PropTypes.number,
        id: PropTypes.string
    }),
    getHandleProps: PropTypes.func
};

export default Handle;
