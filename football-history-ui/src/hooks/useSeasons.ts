import { useEffect, useState } from "react";

export interface Division {
  name: string;
  tier: number;
}

export interface Season {
  startYear: number;
  endYear: number;
  divisions: Division[];
}

const useSeasons = () => {
  const [seasons, setSeasons] = useState<Season[]>();
  const [divisions, setDivisions] = useState<Division[]>([]);

  useEffect(() => {
    fetch(`https://localhost:5001/api/Season/GetSeasons`)
      .then((response) => response.json())
      .then((response) => setSeasons(response))
      .catch(console.log);
  }, []);

  useEffect(() => {
    if (seasons === undefined) {
      return;
    }

    const tiers = Array.from(
      new Set(
        seasons
          .map((s) => s.divisions)
          .flat()
          .map((d) => d.tier)
      )
    );
    let newDivisions = [];
    for (let tier of tiers) {
      const divs = Array.from(
        new Set(
          seasons
            .map((s) => s.divisions)
            .flat()
            .filter((d) => d.tier === tier)
            .map((d) => d.name)
        )
      );
      newDivisions.push({ name: divs.join(", "), tier: tier });
    }

    setDivisions(newDivisions);
  }, [seasons]);

  return { seasons, divisions };
};

export { useSeasons };
