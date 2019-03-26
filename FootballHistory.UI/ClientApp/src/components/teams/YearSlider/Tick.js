import React from "react";
import PropTypes from "prop-types";
import "./Tick.css";

function Tick(props) {
    function getLabel() {
        let seasonStartYear = props.tick.value;
        let startYear = getShortYear(seasonStartYear);
        let endYear = getShortYear(seasonStartYear + 1);
        
        return `${startYear}/${endYear}`;
    }
    
    function getShortYear(year) {
        return year.toString().slice(-2);
    }

    return (
        <div>
            <div className="tick--mark" style={{left: `${props.tick.percent}%`}} />
            <div className='tick--label'
                style={{
                    marginLeft: `${(100 / props.count) / -2}%`,
                    width: `${100 / props.count}%`,
                    left: `${props.tick.percent}%`
                }}
            >
                { getLabel() }
            </div>
        </div>
    )
}

Tick.propTypes = {
    tick: PropTypes.shape({
        percent: PropTypes.number,
        value: PropTypes.number
    }),
    count: PropTypes.number
};

export default Tick;
