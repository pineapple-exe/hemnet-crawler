import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import Home from './components/Home.js';
import './custom.css';
import Listings from './components/Listings.js';
import FinalBids from './components/FinalBids.js';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/listings' component={Listings} />
        <Route path='/finalBids' component={FinalBids} />
      </Layout>
    );
  }
}
