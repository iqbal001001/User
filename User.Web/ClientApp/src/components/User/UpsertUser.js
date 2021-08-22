import React, { Component } from 'react';
import LayoutContext from '../LayoutContext';
import { FormErrors }   from '../FormError';
import UserService from '../../Service/UserService';
import validator from 'validator'

const usersrv = UserService();

export class UpsertUser extends Component {
	static displayName = UpsertUser.name;
	static contextType = LayoutContext;

	constructor(props) {
		super(props);
		this.state = {
			user: {
				id : 0,
				firstName: '',
				lastName: '',
				email: '',
				gender: '',
				status: false
			},
			loading: true,
			mode: this.props.match.params.id === undefined ? "add" : "edit",
			genders: ["Male", "Female", "Other"],
			formErrors: { firstName: '', lastName: '', email: '', gender: '' },
			firstNameValid: false,
			lastNameValid: false,
			emailValid: false,
			genderValid: false,
			statusValid: false,
			controller: null
		};
	}

	componentDidMount() {
		if (this.state.mode === "edit") {
			this.populateUserData(this.props.match.params.id);
		} else {
			var User = { firstName: "", lastName: "", email: "", gender: "", active: false };
			this.setState({ User, loading: false });
		}
	}

	handleChange = (value) => (prop) => {
		console.log(value);
		var p = { ...this.state.user }
		p[prop] = value
		console.log(p);
		this.setState({ user: p }, () => { this.validateField(prop, value) });
	}

	validateField(fieldName, value) {
		let fieldValidationErrors = this.state.formErrors;
		let firstNameValid = this.state.firstNameValid;
		let lastNameValid = this.state.lastNameValid;
		let emailValid = this.state.emailValid;
		let genderValid = this.state.genderValid;

		switch (fieldName) {
			case 'firstName':
				firstNameValid = value.length > 0;
				fieldValidationErrors.firstName = firstNameValid ? '' : ' is invalid';
				break;
			case 'lastName':
				lastNameValid = value.length > 0;
				fieldValidationErrors.lasttName = lastNameValid ? '' : ' is invalid';
				break;
			case 'email':
				emailValid = validator.isEmail(value);
				fieldValidationErrors.email = emailValid ? '' : ' is invalid';
				break;
			case 'gender':
				genderValid = this.state.genders.includes(value);
				fieldValidationErrors.gender = genderValid ? '' : ' is invalid';
				break;
			default:
				break;
		}
		this.setState({
			formErrors: fieldValidationErrors,
			firstNameValid: firstNameValid,
			lastNameValid: lastNameValid,
			emailValid: emailValid,
			genderValid: genderValid
		}, this.validateForm);
	}

	validateForm() {
		console.log(this.state.firstNameValid);
		console.log(this.state.lastNameValid);
		console.log(this.state.emailValid);
		console.log(this.state.genderValid);

		this.setState({ formValid: this.state.firstNameValid && this.state.lastNameValid && this.state.emailValid && this.state.genderValid });
	}

	errorClass(error) {
		return (error.length === 0 ? '' : 'has-error');
	}

	onSubmit = async (e) => {
		e.preventDefault();
		if (this.state.controller)
			this.state.controller.abort(); //cancel the previous request

		let newController = new AbortController();
		let signal = newController.signal;

		this.setState({ controller: newController });

		if (this.state.mode === "edit") {
			await this.saveUserData(signal);
		} else {
			await this.createUserData(signal);
		}
	}

	onBack = async () => {
		this.props.history.push("/");
	}


	renderUser = (User) => {
		return (
			<div>
	
				<form  noValidate>
					<div className="panel panel-default">
						<FormErrors formErrors={this.state.formErrors} />
					</div>
					<div className={`form-group col-6 ${this.errorClass(this.state.formErrors.firstName)}`} >
						<label htmlFor="firstName">First Name</label>
						<input type="text" className="form-control" id="firstName" placeholder="First Name" required
							value={User.firstName}
							onChange={(e) => { this.handleChange(e.target.value)("firstName"); }}
						/>
					</div>
					<div className={`form-group col-6 ${this.errorClass(this.state.formErrors.lastName)}`} >
						<label htmlFor="lastName">Last Name</label>
						<input type="text" className="form-control" id="lastName" placeholder="Last Name" required
							value={User.lastName}
							onChange={(e) => { this.handleChange(e.target.value)("lastName"); }}
						/>
					</div>
					<div className={`form-group col-6 ${this.errorClass(this.state.formErrors.email)}`}>
						<label htmlFor="email">Email</label>
						<input type="text" className="form-control" id="email" placeholder="email"
							value={User.email}
							onChange={(e) => { this.handleChange(e.target.value)("email"); }}

						/>
					</div>
					<div className={`form-group col-6 ${this.errorClass(this.state.formErrors.gender)}`}>
						<select className="select-css"
							value={User.gender}
							onChange={(e) => {
								this.handleChange(e.target.value)("gender");
							}}
						>
							<option defaultValue>Choose Gender</option>
							{this.state.genders.map((g) =>
								<option key={g} value={g}>{g}</option>
							)}
						</select>
					</div>
					<div className="form-group col-6">
						<label className="switch">
							<input type="checkbox" checked={User.status}
								onChange={(e) => { this.handleChange(!this.state.User.status)("status"); }}

							/>t
						<span className="slider round"></span>
						</label>
					</div>
					<div className="form-group row">
						<div className="col-3">
							<button className="btn btn-primary" disabled={!this.state.formValid}
								onClick={(e) => { this.onSubmit(e) }} type="submit">Save</button>
						</div>
						<div className="col-3">
							<button className="btn btn-primary" onClick={() => { this.onBack() }}>Back</button>
						</div>
					</div>
				</form>
			</div>
		);
	}

	render() {
		let contents = this.state.loading
			? <p><em>Loading...</em></p>
			: this.renderUser(this.state.user);

		return (
			<div>
				<h1 id="tabelLabel" >User</h1>
				{contents}
			</div>
		);
	}

	async populateUserData(id, signal) {
		try {
			const data = await usersrv.getUser(id, signal);
			console.log(data);
			this.setState({
				user: data, loading: false,
				formValid: true,
				firstNameValid: true,
				lastNameValid: true,
				emailValid: true,
				genderValid: true, });
		} catch (error) {
			console.log('Fetch error: ', error);
		}
	}

	async saveUserData(signal) {
		try {
			const response = await usersrv.updateUser(this.state.user, signal);
			console.log(response);
			this.context.notify("saved User " + this.state.user.firstName);
			//const data = await response.json();
		} catch (error) {
			console.log('Fetch error: ', error);
		}
	}

	async createUserData(signal) {
		try {
			const response = await usersrv.createUser(this.state.user, signal);
			const data = await response.json();
			console.log(response);
			if (response.status === 201) {
				this.context.notify("created User " + this.state.user.firstName);
				var p = { ...this.state.User, id: data.id };
				this.setState({ user: p });
				this.props.history.push("/EditUser/" + p.id);
			}
		} catch (error) {
			console.log('Fetch error: ', error);
		}
	}
}
