import React, { Component } from 'react';
import { Nav, Navbar } from 'react-bootstrap';

export class NavMenu extends Component {
  displayName = NavMenu.name

  render() {
    return (
      <Navbar bg='primary' variant='dark' sticky='top' collapseOnSelect>
        <Navbar.Brand href='/'>Football History</Navbar.Brand>
        <Navbar.Toggle />
        <Navbar.Collapse>
          <Nav>
              <Nav.Link href={'/seasons'}>Seasons</Nav.Link>
              <Nav.Link href={'/teams'}>Teams</Nav.Link>
          </Nav>
        </Navbar.Collapse>
      </Navbar>
    );
  }
}
