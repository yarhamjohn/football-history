import { useEffect, useState } from "react";

export interface Club {
  name: string;
  abbreviation: string;
}

const useClubs = () => {
  const [clubs, setClubs] = useState<Club[]>([]);

  useEffect(() => {
    fetch(`https://localhost:5001/api/Team/GetAllTeams`)
      .then((response) => response.json())
      .then((response) => setClubs(response))
      .catch(console.log);
  }, []);

  return { clubs };
};

export { useClubs };
