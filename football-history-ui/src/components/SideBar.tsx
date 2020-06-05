import { Icon, Menu } from "semantic-ui-react";
import React, { FunctionComponent } from "react";
import { AppPage, AppSubPage } from "../App";

const SideBar: FunctionComponent<{
  activePage: AppPage;
  activeSubPage: AppSubPage;
  setActiveSubPage: (aubPage: AppSubPage) => void;
}> = ({ activePage, activeSubPage, setActiveSubPage }) => {
  if (activeSubPage === "None") {
    return null;
  }

  if (activePage === "Club") {
    return (
      <Menu compact icon="labeled" vertical pointing>
        <Menu.Item
          name="Positions"
          active={activeSubPage === "Positions"}
          onClick={() => setActiveSubPage("Positions")}
        >
          <Icon name="chart line" />
          Positions
        </Menu.Item>
        <Menu.Item
          name="Table"
          active={activeSubPage === "Table"}
          onClick={() => setActiveSubPage("Table")}
        >
          <Icon name="table" />
          Table
        </Menu.Item>
        <Menu.Item
          name="Results"
          active={activeSubPage === "Results"}
          onClick={() => setActiveSubPage("Results")}
        >
          <Icon name="calendar alternate" />
          Results
        </Menu.Item>
      </Menu>
    );
  }

  if (activePage === "League") {
    return (
      <Menu compact icon="labeled" vertical pointing>
        <Menu.Item
          name="Table"
          active={activeSubPage === "Table"}
          onClick={() => setActiveSubPage("Table")}
        >
          <Icon name="table" />
          Table
        </Menu.Item>

        <Menu.Item
          name="Results"
          active={activeSubPage === "Results"}
          onClick={() => setActiveSubPage("Results")}
        >
          <Icon name="calendar alternate" />
          Results
        </Menu.Item>
      </Menu>
    );
  }

  return null;
};

export { SideBar };
