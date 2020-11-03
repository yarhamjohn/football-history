import React, { FunctionComponent, useEffect, useState } from "react";
import { Divider } from "semantic-ui-react";
import { Season } from "../hooks/useSeasons";
import { DivisionFilter } from "../components/Filters/LeagueFilter";
import { AppSubPage } from "../App";
import { SeasonFilter } from "../components/Filters/SeasonFilter";
import { Matches } from "../components/Matches";
import { LeagueTable } from "../components/LeagueTable/LeagueTable";

const LeaguePage: FunctionComponent<{
    seasons: Season[];
    activeSubPage: AppSubPage;
    setActiveSubPage: (subPage: AppSubPage) => void;
}> = ({ seasons, activeSubPage, setActiveSubPage }) => {
    const [selectedDivision, setSelectedDivision] = useState<string | undefined>(undefined);
    const [selectedSeason, setSelectedSeason] = useState<number | undefined>(undefined);
    const [selectedTier, setSelectedTier] = useState<number | undefined>(undefined);
    const [divisions, setDivisions] = useState<{ name: string; tier: number }[]>([]);

    useEffect(() => {
        if (divisions.length === 0 || !selectedDivision) {
            return;
        }

        const divs = divisions.filter((d) => d.name === selectedDivision);
        if (divs.length !== 1) {
            throw new Error(
                `The division name (${selectedDivision}) provided matches more than one tier.`
            );
        }

        setSelectedTier(divs[0].tier);
    }, [selectedDivision, divisions]);

    useEffect(() => {
        setSelectedSeason(Math.max(...seasons.map((s) => s.startYear)));
    }, [seasons]);

    useEffect(() => {
        const tiers = Array.from(
            new Set(
                seasons
                    .map((s) => s.divisions)
                    .flat()
                    .map((d) => d.tier)
            )
        );

        let divisions = [];
        for (let tier of tiers) {
            const divs = Array.from(
                new Set(
                    seasons
                        .sort((a, b) => b.startYear - a.startYear)
                        .map((s) => s.divisions)
                        .flat()
                        .filter((d) => d.tier === tier)
                        .map((d) => d.name)
                )
            );
            divisions.push({ name: divs.join(", "), tier: tier });
        }

        setDivisions(divisions);
    }, [seasons]);

    if (seasons.length === 0) {
        return null;
    }

    let body;
    if (activeSubPage === "Table") {
        body = selectedDivision && (
            <>
                <SeasonFilter
                    seasons={seasons}
                    selectedSeason={selectedSeason}
                    selectSeason={(startYear) => setSelectedSeason(startYear)}
                />
                {selectedSeason && selectedTier && (
                    <LeagueTable selectedSeason={selectedSeason} selectedTier={selectedTier} />
                )}
            </>
        );
    } else if (activeSubPage === "Results") {
        body = selectedDivision && (
            <div style={{ display: "grid", gridGap: "1rem" }}>
                <SeasonFilter
                    seasons={seasons}
                    selectedSeason={selectedSeason}
                    selectSeason={(startYear) => setSelectedSeason(startYear)}
                />
                {selectedSeason && selectedTier ? (
                    <Matches selectedSeason={selectedSeason} selectedTier={selectedTier} />
                ) : null}
            </div>
        );
    }

    return (
        <>
            <DivisionFilter
                divisions={divisions}
                selectedDivision={selectedDivision}
                selectDivision={(name) => {
                    setActiveSubPage(name ? "Table" : "None");
                    setSelectedDivision(name);
                }}
            />
            <Divider />
            {body}
        </>
    );
};

export { LeaguePage };
