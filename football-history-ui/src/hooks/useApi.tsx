import React, { FunctionComponent, useContext } from "react";

const ApiContext = React.createContext<string>("https://localhost:5001");

const ApiProvider: FunctionComponent = (props) => {
  const api =
    process.env.NODE_ENV === "development"
      ? "https://localhost:5001"
      : "https://footballhistoryapi.azurewebsites.net";

  return <ApiContext.Provider value={api}>{props.children}</ApiContext.Provider>;
};

const useApi = () => useContext(ApiContext);

export { ApiProvider, useApi };
