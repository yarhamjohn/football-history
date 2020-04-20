import React, { FunctionComponent, useState } from "react";
import "./App.css";
import { HomePage } from "./HomePage";
import { AppHeader } from "./components/AppHeader";
import { ClubPage } from "./ClubPage";
import { Image } from "semantic-ui-react";
import soccerBall from "./images/Soccer-Ball-icon.png";

const App: FunctionComponent = () => {
  const [activePage, setActivePage] = useState<"Home" | "Club">("Home");

  return (
    <div
      style={{
        height: "100%",
        display: "grid",
        gridTemplateColumns: "75px auto 75px",
        gridTemplateRows: "auto 1fr 50px",
        gridTemplateAreas: "'icon header .' 'leftGutter main rightGutter' 'footer footer footer",
      }}
    >
      <Image src={soccerBall} fluid style={{ gridArea: "icon", padding: "10px" }} />
      <AppHeader
        activePage={activePage}
        setActivePage={(page) => setActivePage(page)}
        style={{ gridArea: "header" }}
      />
      <div style={{ gridArea: "main" }}>{activePage === "Home" ? <HomePage /> : <ClubPage />}</div>
      <div
        style={{
          gridArea: "footer",
          backgroundImage:
            "linear-gradient(to bottom, #FFFFFF, #CCF0EE, #99E1DE, #66D2CD, #32C3BD, #00B5AD)",
        }}
      ></div>
    </div>
  );
};

export default App;
