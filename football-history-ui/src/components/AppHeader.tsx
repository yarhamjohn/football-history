import React, { FunctionComponent, useState } from "react";
import { Image, Menu } from "semantic-ui-react";
import soccerBall from "../images/Soccer-Ball-icon.png";

const AppHeader: FunctionComponent<{ activePage: "Home" | "Club", setActivePage: (activePage: "Home" | "Club") => void }> = ({ activePage, setActivePage }) => {
  return (
    <>
      <Image src={soccerBall} style={{ gridArea: "icon", margin: "9px" }} />
      <Menu
        color={"teal"}
        pointing
        secondary
        style={{ gridArea: "header", margin: "5px 25px 5px 15px" }}
      >
        <Menu.Item name="Home" active={activePage === "Home"} onClick={() => setActivePage("Home")}>
          Home
        </Menu.Item>
        <Menu.Item name="Club" active={activePage === "Club"} onClick={() => setActivePage("Club")}>
          Club
        </Menu.Item>
      </Menu>
    </>
  );
};

export { AppHeader };
