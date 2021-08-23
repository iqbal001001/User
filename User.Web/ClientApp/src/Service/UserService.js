

export default function UserService() {

	function getNewUser() {
		return {
			Id: 0
		};
	}

	async function getUsers(api, signal) {
		console.log(api);
		let attr = {};
		if (signal) attr = { signal };
		try {
			const response = await fetch(api, attr);
			console.log(response);
			let pagination = JSON.parse(response.headers.get('X-Pagination'));
			console.log(pagination);
			const data = await response.json();
			return { Users: data, loading: false, pagination };
		} catch (error) {
			console.log('service Fetch error: ', error);
		}
	}

    async function getUser(id, signal) {
        let attr = {};
        if (signal) attr = { signal };
        try {
            const response = await fetch('User/' + id, attr);
            const data = await response.json();
            console.log(data);
            return data;
        } catch (error) {
            console.log('Fetch error: ', error);
        }
    }

    async function deleteUser(id, signal) {
        let attr = {};
        if (signal) attr = { signal };
        try {
            const requestOptions = {
                method: 'DELETE',
                headers: { 'Content-Type': 'application/json' }
            };
            const response = await fetch('User/' + id, requestOptions);
            console.log(response);
            return response;
        } catch (error) {
            console.log('Fetch error: ', error);
        }
    }


    async function updateUser(User, signal) {
        let attr = {};
        if (signal) attr = { signal };
        try {
            const requestOptions = {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    Id: User.id,
                    FirstName: User.firstName,
                    LastName: User.lastName,
                    Email: User.email,
                    Gender: User.gender,
                    Status: !!User.status
                }),
                ...attr
            };
            const response = await fetch('User/' + User.id, requestOptions);
            console.log(response);
            return response;
        } catch (error) {
            console.log('Fetch error: ', error);
        }
    }

    async function createUser(User, signal) {
        let attr = {};
        if (signal) attr = { signal };
        try {
            const requestOptions = {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    FirstName: User.firstName,
                    LastName: User.lastName,
                    Email: User.email,
                    Gender: User.gender,
                    Status: !! User.status
                }),
                ...attr
            };
            console.log(requestOptions.body);
            let response = await fetch('User/', requestOptions);
            return response;
            
        } catch (error) {
            console.log('Fetch error: ', error);
        }
    }

	return {
		getNewUser,
        getUsers,
        getUser,
        createUser,
        updateUser,
        deleteUser
	}


}