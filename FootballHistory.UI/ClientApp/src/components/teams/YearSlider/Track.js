import React from "react";
import PropTypes from "prop-types";
import "./Track.css";

function Track(props) {
    return (
        <div className="track--bar"
            style={{
                left: `${props.source.percent}%`,
                width: `${props.target.percent - props.source.percent}%`
            }}
            {...props.getTrackProps()}
        />
    )
}

Track.propTypes = {
    source: PropTypes.shape({
        percent: PropTypes.number
    }),
    target: PropTypes.shape({
        percent: PropTypes.number
    }),
    getTrackProps: PropTypes.func
};

export default Track;
