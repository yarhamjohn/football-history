import React, { FunctionComponent, useEffect, useState } from "react";
import "./App.css";
import { HomePage } from "./HomePage";
import { AppHeader } from "./components/AppHeader";
import { ClubPage } from "./ClubPage";
import { Icon, Image, Menu } from "semantic-ui-react";
import soccerBall from "./images/Soccer-Ball-icon.png";
import { LeaguePage } from "./LeaguePage";
import { useClubs } from "./hooks/useClubs";
import { useSeasons } from "./hooks/useSeasons";
import { SideBar } from "./components/SideBar";

export type AppPage = "Home" | "Club" | "League";
export type AppSubPage = "None" | "Table" | "Results" | "Positions";

const App: FunctionComponent = () => {
  const { seasonState } = useSeasons();
  const [activePage, setActivePage] = useState<AppPage>("Home");
  const [activeSubPage, setActiveSubPage] = useState<AppSubPage>("None");

  useEffect(() => {
    setActiveSubPage("None");
  }, [activePage]);

  return (
    <div
      style={{
        height: "100%",
        display: "grid",
        gridTemplateColumns: "1fr 75px 10fr minmax(75px, 1fr)",
        gridTemplateRows: "auto 1fr 100px",
        gridTemplateAreas:
          "'. icon header .' 'leftGutter leftGutter main rightGutter' 'footer footer footer footer",
      }}
    >
      <Image src={soccerBall} fluid style={{ gridArea: "icon", padding: "10px" }} />
      <AppHeader
        activePage={activePage}
        setActivePage={(page) => setActivePage(page)}
        style={{ gridArea: "header" }}
      />
      <div style={{ gridArea: "leftGutter" }}>
        <SideBar
          activePage={activePage}
          activeSubPage={activeSubPage}
          setActiveSubPage={setActiveSubPage}
        />
      </div>
      <div style={{ gridArea: "main" }}>
        {activePage === "Home" ? (
          <HomePage />
        ) : (
          seasonState.type === "SEASONS_LOAD_SUCCEEDED" &&
          (activePage === "Club" ? (
            <ClubPage
              seasons={seasonState.seasons}
              activeSubPage={activeSubPage}
              setActiveSubPage={setActiveSubPage}
            />
          ) : (
            <LeaguePage
              seasons={seasonState.seasons}
              activeSubPage={activeSubPage}
              setActiveSubPage={setActiveSubPage}
            />
          ))
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
