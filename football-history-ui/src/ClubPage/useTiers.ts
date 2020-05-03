import { useState } from "react";

const useTiers = () => {
  const [tier, setTier] = useState<number>(0);

  const getTier = (club: string, seasonStartYear: number) => {
    fetch(`https://localhost:5001/api/Team/GetTier?seasonStartYear=${seasonStartYear}&team=${club}`)
      .then((response) => response.json())
      .then((response) => setTier(response))
      .catch(console.log);
  };

  return { tier, getTier };
};

export { useTiers };
