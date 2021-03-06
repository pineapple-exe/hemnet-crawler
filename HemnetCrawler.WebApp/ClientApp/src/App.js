import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import Home from './components/Home.js';
import './custom.css';
import Listings from './components/Listings.js';
import FinalBids from './components/FinalBids.js';
import Listing from './components/Listing.js';
import FinalBid from './components/FinalBid.js';
import FinalBidEstimation from './components/FinalBidEstimation';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/listings' component={Listings} />
        <Route path='/finalBids' component={FinalBids} />
        <Route path='/listing/:id' component={Listing} />
        <Route path='/finalBid/:id' component={FinalBid} />
        <Route path='/finalBidEstimation/:id' component={FinalBidEstimation} />
      </Layout>
    );
  }
}
