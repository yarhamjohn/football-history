import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/home-page/Home';
import { Leagues } from './components/leagues-page/Leagues';

export default class App extends Component {
  displayName = App.name

  render() {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route exact path='/leagues' component={Leagues} />
      </Layout>
    );
  }
}
