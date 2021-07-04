import React, { FunctionComponent, useEffect, useState } from "react";
import "./App.css";
import { HomePage } from "./HomePage";
import { AppHeader } from "./components/AppHeader";
import { TeamPage } from "./TeamPage";
import { Icon } from "semantic-ui-react";
import { LeaguePage } from "./LeaguePage";
import { useAppDispatch } from "../reduxHooks";
import { fetchSeasons } from "./shared/seasonsSlice";
import { fetchTeams } from "./shared/teamsSlice";

export type AppPage = "Home" | "Team" | "League";

const App: FunctionComponent = () => {
  const dispatch = useAppDispatch();

  const [activePage, setActivePage] = useState<AppPage>("Home");

  useEffect(() => {
    dispatch(fetchSeasons());
    dispatch(fetchTeams());
  }, [dispatch]);

  return (
    <div
      style={{
        height: "100%",
        display: "grid",
        gridTemplateColumns: "minmax(100px, 1fr) minmax(500px, 10fr) minmax(75px, 1fr)",
        gridTemplateRows: "80px 1fr 100px",
        gridTemplateAreas: "'icon header .' 'leftGutter main rightGutter' 'footer footer footer",
      }}
    >
      <div style={{ display: "flex", justifyContent: "flex-end" }}>
        <Icon
          size="huge"
          name="futbol"
          style={{ gridArea: "icon", padding: "5px", marginRight: "23px" }}
        />
      </div>
      <AppHeader
        activePage={activePage}
        setActivePage={(page) => setActivePage(page)}
        style={{ gridArea: "header" }}
      />

      <div style={{ gridArea: "main" }}>
        {activePage === "Home" ? (
          <HomePage />
        ) : activePage === "Team" ? (
          <TeamPage />
        ) : (
          <LeaguePage />
        )}
      </div>
      <div
        style={{
          gridArea: "footer",
          backgroundImage:
            "linear-gradient(to bottom, #CCF0EE, #99E1DE, #66D2CD, #32C3BD, #00B5AD)",
        }}
      ></div>
    </div>
  );
};

export default App;
