import React, {useEffect, useState} from "react";
import baseUrl from "../../api/LeagueSeasonApi";
import {LineChart} from "react-chartkick";
import {Spinner} from "react-bootstrap";

function HistoricalPositions(props) {
    const [selectedTeam, setSelectedTeam] = useState("");
    const [historicalPositions, setHistoricalPositions] = useState([]);
    const [isLoading, setIsLoading] = useState(null);

    useEffect(() => {
        setSelectedTeam(props.selectedTeam);
    });
    
    useEffect(() => {
        setIsLoading(true);
        fetch(`${baseUrl}/api/Team/GetHistoricalPositions?team=${selectedTeam}&firstSeasonStartYear=${1992}&lastSeasonStartYear=${2017}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Failed to fetch')
                }

                return response.json()
            })
            .then(data => {
                setIsLoading(false);
                setHistoricalPositions(data)
            })
            .catch(err => {
                console.log(err);
                setIsLoading(false);
            })
        }, [selectedTeam]);

    let seriesData = [{name: "Positions", data: AddAllPositionsSeriesData()}];
    let colors = ["#0000FF"];

    AddPromotionPositions();
    AddRelegationPositions();

    //TODO: fix tooltip text
    return (
            isLoading 
                ? <Spinner animation='border' variant='info' /> 
                : <div style={{display: 'flex'}}>
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

    function GetSeasonAbbreviation(season) {
        return `${season.substring(2, 4)} - ${season.substring(9, 11)}`;
    }

    function AddAllPositionsSeriesData() {
        return historicalPositions.reduce(function (map, pos) {
            if (pos.absolutePosition === 0) {
                map[GetSeasonAbbreviation(pos.season)] = null;
            }
            else {
                map[GetSeasonAbbreviation(pos.season)] = pos.absolutePosition;
            }
            return map;
        }, {});
    }
    
    function AddPromotionPositions() {
        let promotionPositions = historicalPositions.filter(pos => pos.status === "P" || pos.status === "C" || pos.status === "PO (P)");
        for (let i = 0; i < promotionPositions.length; i++) {
            let data = {};
            data[GetSeasonAbbreviation(promotionPositions[i].season)] = promotionPositions[i].absolutePosition;
            
            seriesData.push(
                {
                    name: `Promotion${i}`,
                    data: data
                });

            colors.push("#00FF00");
        }
    }

    function AddRelegationPositions() {
        let relegationPositions = historicalPositions.filter(pos => pos.status === "R");
        for (let i = 0; i < relegationPositions.length; i++) {
            let data = {};
            data[GetSeasonAbbreviation(relegationPositions[i].season)] = relegationPositions[i].absolutePosition;

            seriesData.push(
                {
                    name: `Relegation${i}`,
                    data: data
                });

            colors.push("#FF0000");
        }
    }
}

export default HistoricalPositions;
