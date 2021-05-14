import React, { FunctionComponent, useState } from "react";
import { useFetchHistoricalPositions } from "../../shared/useFetchHistoricalPositions";
import { Season } from "../../shared/useFetchSeasons";
import { YearSlider } from "../Filters/YearSlider";
import { ErrorMessage } from "../ErrorMessage";
import { HistoricalPositionsGraph } from "./Graph";

export type HistoricalPositionRange = {
  startYear: number;
  endYear: number;
};

const HistoricalPositions: FunctionComponent<{
  teamId: number;
  seasons: Season[];
}> = ({ teamId, seasons }) => {
  const getFirstSeasonStartYear = () => Math.min(...seasons.map((s) => s.startYear));
  const getLastSeasonStartYear = () => Math.max(...seasons.map((s) => s.startYear));

  const [selectedRange, setSelectedRange] = useState<HistoricalPositionRange>({
    startYear: getLastSeasonStartYear() - 25, // To speed initial load, only the last 25 seasons are fetched to begin with);
    endYear: getLastSeasonStartYear(),
  });

  const { state } = useFetchHistoricalPositions(teamId, seasons, selectedRange);

  return (
    <div>
      <YearSlider
        sliderRange={[getFirstSeasonStartYear(), getLastSeasonStartYear()]}
        selectedRange={selectedRange}
        setSelectedRange={setSelectedRange}
      />
      {state.type === "HISTORICAL_POSITIONS_UNLOADED" ? null : state.type ===
        "HISTORICAL_POSITIONS_LOAD_FAILED" ? (
        <ErrorMessage header={"Sorry, something went wrong."} content={state.error} />
      ) : (
        <HistoricalPositionsGraph
          isLoading={state.type === "HISTORICAL_POSITIONS_LOADING"}
          positions={state.positions}
          range={selectedRange}
        />
      )}
    </div>
  );
};

export { HistoricalPositions };
