import React, { FunctionComponent, useEffect, useState } from "react";
import "./App.css";
import { HomePage } from "./HomePage";
import { AppHeader } from "./components/AppHeader";
import { ClubPage } from "./ClubPage";
import { Icon } from "semantic-ui-react";
import { useFetchSeasons } from "./shared/useFetchSeasons";
import { SideBar } from "./components/SideBar";
import { LeaguePage } from "./LeaguePage";

export type AppPage = "Home" | "Club" | "League";
export type AppSubPage = "None" | "Table" | "Results" | "Positions";

const App: FunctionComponent = () => {
  const seasons = useFetchSeasons();
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
      <div style={{ gridArea: "leftGutter", marginTop: "100px" }}>
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
          seasons.status === "LOAD_SUCCESSFUL" &&
          (activePage === "Club" ? (
            <ClubPage
              seasons={seasons.data}
              activeSubPage={activeSubPage}
              setActiveSubPage={setActiveSubPage}
            />
          ) : (
            <LeaguePage
              seasons={seasons.data}
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
