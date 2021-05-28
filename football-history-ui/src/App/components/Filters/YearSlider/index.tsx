import * as React from "react";
import { FunctionComponent, useEffect, useState } from "react";
import { Handles, Rail, Slider, Ticks, Tracks } from "react-compound-slider";
import { Handle, Tick, Track } from "./Components";
import { HistoricalPositionRange } from "../../HistoricalPositions";

const YearSlider: FunctionComponent<{
  sliderRange: number[];
  selectedRange: HistoricalPositionRange;
  setSelectedRange: (range: HistoricalPositionRange) => void;
}> = ({ sliderRange, selectedRange, setSelectedRange }) => {
  const [newSelectedRange, setNewSelectedRange] = useState<HistoricalPositionRange>(selectedRange);

  useEffect(() => setSelectedRange(newSelectedRange), [
    newSelectedRange.startYear,
    newSelectedRange.endYear,
  ]);

  const onChange = (values: readonly number[]) => {
    setNewSelectedRange({ startYear: Math.min(...values), endYear: Math.max(...values) });
  };

  return (
    <div
      style={{
        height: "75px",
        width: "100%",
        display: "flex",
        justifyContent: "center",
        marginBottom: "2rem",
      }}
    >
      <Slider
        mode={1}
        step={1}
        domain={sliderRange}
        rootStyle={{
          margin: "2rem",
          position: "relative",
          width: "90%",
        }}
        onChange={onChange}
        values={[selectedRange.startYear, selectedRange.endYear]}
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
        <Ticks count={sliderRange[1] - sliderRange[0]}>
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
