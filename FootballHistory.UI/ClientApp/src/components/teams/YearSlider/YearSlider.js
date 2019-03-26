import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import { Slider, Rail, Tracks, Ticks, Handles } from "react-compound-slider";
import Track from "./Track";
import Handle from "./Handle";
import Tick from "./Tick";
import "./YearSlider.css";

function YearSlider(props) {
    const [domain, setDomain] = useState([props.rangeStart, props.rangeEnd]);
    const [values, setValues] = useState([props.selectedStart, props.selectedEnd]);

    useEffect(() => {
        setDomain([props.rangeStart, props.rangeEnd]);
    }, []);
    
    useEffect(() => {
        setValues([props.selectedStart, props.selectedEnd]);
    }, [props.selectedStart, props.selectedEnd]);
    
    function onChange(values) {
        setValues(values);
        props.updateSelectedSeasons(values);
    }

    //TODO: figure out how to disable it when loading
    let numTicks = props.rangeEnd - props.rangeStart;
    return (
        <div className="slider--container" style={{opacity: props.disableSlider ? 0.5 : 1}}>
            <Slider mode={1}
                    step={1}
                    disabled={props.disableSlider}
                    domain={domain}
                    rootStyle={{
                        margin: '5%',
                        position: 'relative',
                        width: '90%'
                    }}
                    onChange={props.disableSlider ? () => {} : (selectedValues) => onChange(selectedValues)}
                    values={values}
            >
                <Rail>
                    {({getRailProps}) => (
                        <div className="slider--rail" {...getRailProps()} />
                    )}
                </Rail>
                <Handles>
                    {({handles, getHandleProps}) => (
                        <div>
                            {handles.map(handle => (
                                <Handle
                                    key={handle.id}
                                    handle={handle}
                                    domain={domain}
                                    getHandleProps={getHandleProps} />
                            ))}
                        </div>
                    )}
                </Handles>
                <Tracks left={false} right={false}>
                    {({tracks, getTrackProps}) => (
                        <div>
                            {tracks.map(({id, source, target}) => (
                                <Track
                                    key={id}
                                    source={source}
                                    target={target}
                                    getTrackProps={getTrackProps} />
                            ))}
                        </div>
                    )}
                </Tracks>
                <Ticks count={numTicks}> 
                    {({ticks}) => (
                        <div>
                        {ticks.map(tick =>
                                <Tick 
                                    key={tick.id} 
                                    tick={tick} 
                                    count={ticks.length} />
                            )}
                        </div>
                    )}
                </Ticks>
            </Slider>
        </div>
    )
}

YearSlider.propTypes = {
    rangeStart: PropTypes.number,
    rangeEnd: PropTypes.number,
    selectedStart: PropTypes.number,
    selectedEnd: PropTypes.number,
    disableSlider: PropTypes.bool,
    updateSelectedSeasons: PropTypes.func
};

export default YearSlider;
