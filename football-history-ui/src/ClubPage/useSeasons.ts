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

  useEffect(() => {
    fetch(`https://localhost:5001/api/Season/GetSeasons`)
      .then((response) => response.json())
      .then((response) => setSeasons(response))
      .catch(console.log);
  }, []);

  return { seasons };
};

export { useSeasons };
