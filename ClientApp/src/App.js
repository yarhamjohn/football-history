import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import HomePage from './components/home/Page';
import SeasonsPage from './components/seasons/Page';

export default class App extends Component {
  displayName = App.name

  render() {
    return (
      <Layout>
        <Route exact path='/' component={HomePage} />
        <Route exact path='/seasons' component={SeasonsPage} />
      </Layout>
    );
  }
}
