import * as React from "react";
import { FunctionComponent } from "react";
import { Handles, Rail, Slider, Ticks, Tracks } from "react-compound-slider";
import { Handle, Tick, Track } from "./Components";

const YearSlider: FunctionComponent<{
  sliderRange: number[];
  selectedFilterRange: number[];
  setSelectedFilterRange: (range: number[]) => void;
}> = ({ sliderRange, selectedFilterRange, setSelectedFilterRange }) => {
  const numTicks = sliderRange[1] - sliderRange[0];

  const onChange = (values: readonly number[]) => {
    const newValues = [...values];
    setSelectedFilterRange(newValues);
  };

  return (
    <div style={{ height: "75px", width: "100%" }}>
      <Slider
        mode={1}
        step={1}
        domain={sliderRange}
        rootStyle={{
          margin: "5%",
          position: "relative",
          width: "90%",
        }}
        onChange={onChange}
        values={selectedFilterRange}
      >
        <Rail>
          {({ getRailProps }) => (
            <div
              style={{
                position: "absolute",
                width: "100%",
                height: 14,
                borderRadius: 7,
                cursor: "pointer",
                backgroundColor: "rgb(155,155,155)",
              }}
              {...getRailProps()}
            />
          )}
        </Rail>
        <Handles>
          {({ handles, getHandleProps }) => (
            <div>
              {handles.map((handle) => (
                <Handle
                  key={handle.id}
                  handle={handle}
                  domain={sliderRange}
                  getHandleProps={getHandleProps}
                />
              ))}
            </div>
          )}
        </Handles>
        <Tracks left={false} right={false}>
          {({ tracks, getTrackProps }) => (
            <div>
              {tracks.map(({ id, source, target }) => (
                <Track key={id} source={source} target={target} getTrackProps={getTrackProps} />
              ))}
            </div>
          )}
        </Tracks>
        <Ticks count={numTicks}>
          {({ ticks }) => (
            <div>
              {ticks.map((tick) => (
                <Tick key={tick.id} tick={tick} count={ticks.length} />
              ))}
            </div>
          )}
        </Ticks>
      </Slider>
    </div>
  );
};

export { YearSlider };
