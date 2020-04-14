import React from "react";
import "./App.css";
import { Page } from "./HomePage";
import { AppHeader } from "./components/AppHeader";

function App() {
  return (
    <div
      style={{
        display: "grid",
        gridTemplateColumns: "25px 25px auto 25px",
        gridTemplateRows: "50px auto ",
        gridTemplateAreas: "'icon icon header header' 'leftGutter main main rightGutter'",
      }}
    >
      <AppHeader />
      <div style={{ gridArea: "main" }}>
        <Page />
      </div>
    </div>
  );
}

export default App;
