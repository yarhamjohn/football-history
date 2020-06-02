import { useEffect, useState } from "react";

const useApi = () => {
  const devApiUrl = "https://localhost:5001";
  const prodApiUrl = "https://footballhistoryapi.azurewebsites.net";

  const [api, setApi] = useState<string>(devApiUrl);
  console.log(api);
  useEffect(() => {
    console.log("NODE_ENV");
    console.log(process.env.NODE_ENV);

    if (process.env.NODE_ENV === "development") {
      setApi(devApiUrl);
    } else {
      setApi(prodApiUrl);
    }

    console.log(api);
  }, []);

  return { api };
};

export { useApi };
