import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import HomePage from './components/home/Page';
import LeaguesPage from './components/leagues/Page';

export default class App extends Component {
  displayName = App.name

  render() {
    return (
      <Layout>
        <Route exact path='/' component={HomePage} />
        <Route exact path='/leagues' component={LeaguesPage} />
      </Layout>
    );
  }
}
