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
        <div style={{display: 'flex'}}>
            <div style={{minWidth: '120px', display: 'flex', flexDirection: 'column', justifyContent: 'space-around'}}>
                <strong>Premier League</strong>
                <strong>Championship</strong>
                <strong>League One</strong>
                <strong>League Two</strong>
            </div>
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
                    series: { 
                        2: { targetAxisIndex: 1}
                    },
                    vAxes: {
                        0: {
                            ticks: [
                                4, 8, 12, 16, 20, 
                                {v: 24, f:"4"}, {v: 28, f:"8"}, {v: 32, f:"12"}, {v: 36, f:"16"}, {v: 40, f:"20"}, {v: 44, f:"24"}, 
                                {v: 48, f:"4"}, {v: 52, f:"8"}, {v: 56, f:"12"}, {v: 60, f:"16"}, {v: 64, f:"20"}, {v: 68, f:"24"}, 
                                {v: 72, f:"4"}, {v: 76, f:"8"}, {v: 80, f:"12"}, {v: 84, f:"16"}, {v: 88, f:"20"}, {v: 92, f:"24"}],
                            direction: -1
                        }, 
                        1: {
                            ticks: [20, 44, 68, 92],
                            direction: -1,
                            gridlines: { color: "#000000"},
                            textPosition: 'none'
                        }
                    }
                }}
                />
        </div>
    );
}

export default HistoricalPositions;
