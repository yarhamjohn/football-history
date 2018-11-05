import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import { Leagues } from './components/Leagues';

export default class App extends Component {
  displayName = App.name

  render() {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route exact path='/leagues' component={Leagues} />
        <Route path='/counter' component={Counter} />
        <Route path='/fetchdata' component={FetchData} />
      </Layout>
    );
  }
}
