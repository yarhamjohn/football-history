import React, {useEffect, useState} from "react";
import baseUrl from "../../api/LeagueSeasonApi";
import {LineChart} from "react-chartkick";

function HistoricalPositions(props) {
    const [selectedTeam, setSelectedTeam] = useState(props.selectedState);
    const [historicalPositions, setHistoricalPositions] = useState([]);
    
    useEffect(() => {
        fetch(`${baseUrl}/api/Team/GetHistoricalPositions?team=${selectedTeam}`)
            .then(response => response.json())
            .then(data => {
                setHistoricalPositions(data);
            });
    }, []);
    
    
    let data = historicalPositions.reduce(function(map, pos) {
        map[pos.season] = pos.absolutePosition;
        return map;
    }, {});
    let promotionData = historicalPositions.reduce(function(map, pos) {
        if (pos.status === "P") {
            map[pos.season] = pos.absolutePosition;
        }
        return map;
    }, {});
    let relegationData = historicalPositions.reduce(function(map, pos) {
        if (pos.status === "R") {
            map[pos.season] = pos.absolutePosition;
        }
        return map;
    }, {});

    return (
        <div>
            <LineChart
                data={
                    [{name: "Positions", data: data},
                     {name: "Promotion", data: promotionData},
                     {name: "Relegation", data: relegationData}]
                }
                colors={["#0000FF", "#00FF00", "#FF0000"]}
                min={1}
                max={92}
                legend={false}
                library={{
                    chartArea: {
                        width: '90%',
                        height: '90%'
                    },
                    series: { 2: { targetAxisIndex: 1}},
                    vAxes: {0: {
                        ticks: [4, 8, 12, 16, 20, 24, 28, 32, 36, 40, 44, 48, 52, 56, 60, 64, 68, 72, 76, 80, 84, 88, 92],
                        direction: -1
                    }, 1: {
                            ticks: [20, 44, 68, 92],
                            direction: -1,
                        gridlines: { color: "#FF0000"},
                        textPosition: 'none'}
                }}}
                />
        </div>
    );
}

export default HistoricalPositions;
