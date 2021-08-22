import React, { Component } from 'react';
import { Route } from 'react-router';
import { ErrorBoundary } from './ErrorBoundary';
import { Layout } from './components/Layout';
import { Users } from './components/User/Users';
import { UpsertUser } from './components/User/UpsertUser';

import './custom.css'

export default class App extends Component {
	static displayName = App.name;

	render() {
		return (
			<ErrorBoundary>
				<Layout>
					<Route exact path='/' component={Users} />
					<Route path='/addUser' component={UpsertUser} />
					<Route path='/editUser/:id' component={UpsertUser} />
				</Layout>
			</ErrorBoundary>
		);
	}
}
