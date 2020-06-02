import { useEffect, useState } from "react";

const useApi = () => {
  const devApiUrl = "https://localhost:5001";
  const prodApiUrl = "https://footballhistoryapi.azurewebsites.net";

  const [api, setApi] = useState<string>(devApiUrl);

  useEffect(() => {
    if (process.env.NODE_ENV === "development") {
      setApi(devApiUrl);
    } else {
      setApi(prodApiUrl);
    }
  }, []);

  return { api };
};

export { useApi };
