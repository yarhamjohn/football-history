import React, { Component } from 'react';
import { Nav, Navbar, Image } from 'react-bootstrap';
import soccerBall from '../images/Soccer-Ball-icon.png';
import './NavMenu.css';

export class NavMenu extends Component {
  displayName = NavMenu.name

  render() {
    return (
      <Navbar bg='primary' variant='dark' sticky='top' collapseOnSelect>
        <Image className='image' src={soccerBall}/>
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
