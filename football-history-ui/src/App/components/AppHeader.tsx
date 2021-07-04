import React, { FunctionComponent } from "react";
import { Menu } from "semantic-ui-react";

const AppHeader: FunctionComponent<{
  activePage: "Home" | "Team" | "League";
  setActivePage: (activePage: "Home" | "Team" | "League") => void;
  style: React.CSSProperties;
}> = ({ activePage, setActivePage, style }) => {
  return (
    <Menu
      color={"teal"}
      pointing
      secondary
      style={{ ...style, fontSize: "1.5rem", alignSelf: "center", margin: 0 }}
    >
      <Menu.Item name="Home" active={activePage === "Home"} onClick={() => setActivePage("Home")}>
        Home
      </Menu.Item>
      <Menu.Item name="Team" active={activePage === "Team"} onClick={() => setActivePage("Team")}>
        Club
      </Menu.Item>
      <Menu.Item
        name="League"
        active={activePage === "League"}
        onClick={() => setActivePage("League")}
      >
        League
      </Menu.Item>
    </Menu>
  );
};

export { AppHeader };
