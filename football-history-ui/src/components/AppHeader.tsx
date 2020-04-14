import React, { FunctionComponent, useState } from "react";
import { Image, Menu } from "semantic-ui-react";
import soccerBall from "../images/Soccer-Ball-icon.png";

const AppHeader: FunctionComponent<{ style?: React.CSSProperties }> = ({ style }) => {
  const [activeItem, setActiveItem] = useState<string>("home");

  return (
    <>
      <Image src={soccerBall} style={{ gridArea: "icon", margin: "9px" }} />
      <Menu
        color={"teal"}
        pointing
        secondary
        style={{ gridArea: "header", margin: "5px 25px 5px 15px" }}
      >
        <Menu.Item name="home" active={activeItem === "home"} onClick={() => setActiveItem("home")}>
          Home
        </Menu.Item>
        <Menu.Item name="next" active={activeItem === "next"} onClick={() => setActiveItem("next")}>
          Next Page
        </Menu.Item>
      </Menu>
    </>
  );
};

export { AppHeader };
