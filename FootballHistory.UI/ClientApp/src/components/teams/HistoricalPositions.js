import React, {useEffect, useState} from "react";
import baseUrl from "../../api/LeagueSeasonApi";
import {LineChart} from "react-chartkick";

function HistoricalPositions(props) {
    const [selectedTeam, setSelectedTeam] = useState("");
    const [historicalPositions, setHistoricalPositions] = useState([]);

    useEffect(() => {
        setSelectedTeam(props.selectedTeam);
    });
    
    useEffect(() => {
        fetch(`${baseUrl}/api/Team/GetHistoricalPositions?team=${selectedTeam}`)
            .then(response => response.json())
            .then(data => {
                setHistoricalPositions(data);
            });
    }, [selectedTeam]);

    function GetAllPositions() {
        return historicalPositions.reduce(function (map, pos) {
            map[`${pos.season.substring(2, 4)} - ${pos.season.substring(9, 11)}`] = pos.absolutePosition;
            return map;
        }, {});
    }

    let seriesData = [{name: "Positions", data: GetAllPositions()}];
    let colors = ["#0000FF"];

    let promotionPositions = historicalPositions.filter(pos => pos.status === "P" || pos.status === "C" || pos.status === "PO (P)");
    for (let i = 0; i < promotionPositions.length; i++)
    {
        let test = {};
        test[`${promotionPositions[i].season.substring(2, 4)} - ${promotionPositions[i].season.substring(9, 11)}`] = promotionPositions[i].absolutePosition; 
        seriesData.push(
            {
                name: `Promotion${i}`,
                data: test
            });
        
        colors.push("#00FF00");
    }

    let relegationPositions = historicalPositions.filter(pos => pos.status === "R");
    for (let i = 0; i < relegationPositions.length; i++)
    {
        let test = {};
        test[`${relegationPositions[i].season.substring(2, 4)} - ${relegationPositions[i].season.substring(9, 11)}`] = relegationPositions[i].absolutePosition;

        seriesData.push(
            {
                name: `Relegation${i}`,
                data: test
            });
        
        colors.push("#FF0000");
    }

    return (
        <div style={{display: 'flex'}}>
            <div style={{minWidth: '120px', display: 'flex', flexDirection: 'column', justifyContent: 'space-evenly'}}>
                <strong>Premier League</strong>
                <strong>Championship</strong>
                <strong>League One</strong>
                <strong>League Two</strong>
            </div>
            <LineChart
                data={seriesData}
                colors={colors}
                min={1}
                max={92}
                legend={false}
                library={{
                    chartArea: {
                        width: '90%',
                        height: '75%'
                    },
                    series: { 
                        0: { targetAxisIndex: 1}
                    },
                    hAxis: {
                        slantedText: true
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
