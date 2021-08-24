import React, { Component } from 'react';
import { confirmAlert } from 'react-confirm-alert'; 
import 'react-confirm-alert/src/react-confirm-alert.css'; 
import { Pagination } from '../Pagination';
import { DebounceInput } from 'react-debounce-input';
import UserService from '../../Service/UserService';

const usersrv = UserService();

export class Users extends Component {
	static displayName = Users.name;

	constructor(props) {
		super(props);
		this.state = {
			Users: [], UserFilter: {}, loading: true, pagination: {}, sortColumn: 'id', controller: null
			};
	}



	tableFields = [
		{ name: "firstName", type: "string" },
		{ name: "lastName", type: "string" },
		{ name: "email", type: "string" },
		{ name: "gender", type: "string" },
		{ name: "status", type: "boolean" }
	];


	componentDidMount() {
		this.populateUserData('user');
	}

	handlePaginationapi = (api) => {
		console.log(api);
		const params = (new URL(api));
		var queryParams = new URLSearchParams(params.search);
		if (this.state.sortColumn != "") {
			queryParams.set("sort", this.state.sortColumn);
		} else {
			queryParams.delete("sort");
		}
		queryParams.set("jsonSearchFilters", JSON.stringify(this.state.UserFilter));
		console.log(queryParams.toString());
		let link = `${params.origin}${params.pathname}?${queryParams.toString()}`;
		this.populateUserData(link);
	}

	handleClickAdd = () => {
		console.log("Add");
		this.props.history.push("/AddUser/");
	}

	handleClickEdit = (id) => {
		console.log(id);
		this.props.history.push("/EditUser/" + id);
	}

	handleClickDelete = (id) => (name) => {
		confirmAlert({
			title: 'Confirm to Delete',
			message: 'Are you sure you wish to delete '+ name + ' ?',
			buttons: [
				{
					label: 'Yes',
					onClick: () => this.deleteUserData(id)
				},
				{
					label: 'No'
				}
			]
		});
	}

	handleSort = (col) => {
		console.log(col);


		this.setState({ sortColumn: col});
		this.populateUserData(`user?sort=${col}&jsonSearchFilters=${JSON.stringify(this.state.UserFilter)}`);
	}

	handleFilterChange = (e) => (type) => {
		let { name, value } = e.target;
		console.log(value);
		let pair = {};
		if (type == "boolean") {
			if (value == "yes") {
				pair[name] = true ;
			} else if (value == "no") {
				pair[name] = false;
			} else {
				pair[name] = "";
			}
		} else {
			pair[name] = value;
		}

		console.log(pair);
		var filter = { ...this.state.UserFilter, ...pair };
		
		console.log(filter);

		this.setState({ UserFilter: filter });

		this.populateUserData(`user?sort=${this.state.sortColumn}&jsonSearchFilters=${JSON.stringify(filter)}`);
	}


	CreateFilterInput = (field) => {
		return (
			<DebounceInput type="text" className="form-control"
				name={field.name}
				value={this.state.UserFilter[field.name]}
				debounceTimeout={300}
				onChange={(e) => {
						this.handleFilterChange(e)(field.type);
				}}

			/>
			);
	}


	renderUsers = (Users) => {
		return (
			<div>
				<button className="btn btn-primary" onClick={() => this.handleClickAdd()} >Add</button>
				<p> Click on header to sort</p>
				<table className='table table-striped' aria-labelledby="tabelLabel">
					<thead>
						<tr>
							{this.tableFields.map((field) => {
								return (<th key={ field.name } onClick={() => this.handleSort(field.name)}>{field.name}</th>);
							})}
							<th></th>
							<th></th>
						</tr>
						<tr>
							{this.tableFields.map((field) => {
								return (<th key={field.name}> {this.CreateFilterInput(field)}</th> );
							})}
							<th></th>
							<th></th>
						</tr>
					</thead>
					<tbody>
						{Users && Users.map(User =>
							<tr key={User.id}>
								<td>{User.firstName}</td>
								<td>{User.lastName}</td>
								<td>{User.email}</td>
								<td>{User.gender}</td>
								<td>{User.status === true ? 'Yes' : 'No'}</td>
								<td><button className="btn btn-primary" onClick={() => this.handleClickEdit(User.id)} >Edit</button></td>
								<td><button className="btn btn-primary" onClick={() => this.handleClickDelete(User.id)(User.firstName)}>Delete</button>
								</td>
							</tr>
						)}
					</tbody>
				</table>
				<Pagination value={this.state.pagination} onChangePaginationapi={this.handlePaginationapi}/>
			</div>
		);
	}

	render() {
		let contents = this.state.loading
			? <p><em>Loading...</em></p>
			: this.renderUsers(this.state.Users);

		return (
			<div>
				<h1 id="tabelLabel" >Users</h1>
				{contents}
			</div>
		);
	}

	async populateUserData(api) {
		try {
			if (this.state.controller)
				this.state.controller.abort(); //cancel the previous request

			let newController = new AbortController();
			let signal = newController.signal;

			this.setState({ controller: newController });

			let { Users, pagination} = await usersrv.getUsers(api, signal);
			this.setState({ Users, loading: false, pagination});
		} catch (error) {
			//if error is not cancel token
			//this.setState({ Users: [], loading: false });
			console.log('Fetch error: ', error);
		}
	}

	async deleteUserData(id, signal) {
		try {
			const response = await usersrv.deleteUser(id, signal);
			if (response.status === 204) {
				let data = this.state.Users.filter((p) => p.id !== id);
				console.log(data);
				this.setState({ Users: data });
			}
		} catch (error) {
			console.log('Fetch error: ', error);
		}
	}
}
