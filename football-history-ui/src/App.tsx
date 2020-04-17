import React, { FunctionComponent, useState } from "react";
import "./App.css";
import { HomePage } from "./HomePage";
import { AppHeader } from "./components/AppHeader";
import { ClubPage } from "./ClubPage";

const App: FunctionComponent = () => {
  const [activePage, setActivePage] = useState<"Home" | "Club">("Home");

  return (
    <div
      style={{
        display: "grid",
        gridTemplateColumns: "75px auto 75px",
        gridTemplateRows: "auto auto ",
        gridTemplateAreas: "'icon header rightGutter' 'leftGutter main rightGutter'",
      }}
    >
      <AppHeader activePage={activePage} setActivePage={(page) => setActivePage(page)} />
      <div style={{ gridArea: "main" }}>{activePage === "Home" ? <HomePage /> : <ClubPage />}</div>
    </div>
  );
};

export default App;