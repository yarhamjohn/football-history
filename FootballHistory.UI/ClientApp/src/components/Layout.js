import React, { Component } from 'react';
import { Col, Container, Row } from 'react-bootstrap';
import { NavMenu } from './NavMenu';

export class Layout extends Component {
  displayName = Layout.name;

  render() {
    return (
        <div>
            <NavMenu />
            <Container fluid>
                <Row style={{paddingTop:'1rem', display:'flex', justifyContent:'space-around'}}>
                    <Col sm={11} lg={10}>
                        <div>
                            {this.props.children}
                        </div>
                    </Col>
                </Row>
            </Container>
        </div>
    );
  }
}
