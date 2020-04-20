import React, { FunctionComponent } from "react";
import { Image, Menu } from "semantic-ui-react";
import soccerBall from "../images/Soccer-Ball-icon.png";

const AppHeader: FunctionComponent<{
  activePage: "Home" | "Club";
  setActivePage: (activePage: "Home" | "Club") => void;
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
      <Menu.Item name="Club" active={activePage === "Club"} onClick={() => setActivePage("Club")}>
        Club
      </Menu.Item>
    </Menu>
  );
};

export { AppHeader };
