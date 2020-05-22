import React, { FunctionComponent, useEffect, useState } from "react";
import "./App.css";
import { HomePage } from "./HomePage";
import { AppHeader } from "./components/AppHeader";
import { ClubPage } from "./ClubPage";
import { Image } from "semantic-ui-react";
import soccerBall from "./images/Soccer-Ball-icon.png";
import { LeaguePage } from "./LeaguePage";
import { useClubs } from "./hooks/useClubs";
import { useSeasons } from "./hooks/useSeasons";

export type AppPage = "Home" | "Club" | "League";

const App: FunctionComponent = () => {
  const { seasonState } = useSeasons();
  const [activePage, setActivePage] = useState<AppPage>("Home");

  return (
    <div
      style={{
        height: "100%",
        display: "grid",
        gridTemplateColumns: "minmax(0, 1fr) 75px 5fr 75px minmax(0, 1fr)",
        gridTemplateRows: "auto 1fr 100px",
        gridTemplateAreas:
          "'. icon header . .' 'leftGutter leftGutter main rightGutter rightGutter' 'footer footer footer footer footer",
      }}
    >
      <Image src={soccerBall} fluid style={{ gridArea: "icon", padding: "10px" }} />
      <AppHeader
        activePage={activePage}
        setActivePage={(page) => setActivePage(page)}
        style={{ gridArea: "header" }}
      />
      <div style={{ gridArea: "main" }}>
        {activePage === "Home" ? (
          <HomePage />
        ) : (
          seasonState.type === "SEASONS_LOAD_SUCCEEDED" &&
          (activePage === "Club" ? (
            <ClubPage seasons={seasonState.seasons} />
          ) : (
            <LeaguePage seasons={seasonState.seasons} />
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
