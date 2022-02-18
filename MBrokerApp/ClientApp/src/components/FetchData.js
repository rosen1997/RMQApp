import React, { Component } from 'react';

export class FetchData extends Component {
    static displayName = FetchData.name;

    constructor(props) {
        super(props);
        this.state = { users: []};

        this.populateUsersData = this.populateUsersData.bind(this);
    }

    componentDidMount() {
        this.populateUsersData();
    }

    static renderUsersTable(users) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>UCN</th>
                        <th>Address</th>
                        <th>Country</th>
                        <th>City</th>
                        <th>Phone Number</th>
                        <th>Gender</th>
                    </tr>
                </thead>
                <tbody>
                    {users.map(user =>
                        <tr key={user.id}>
                            <td>{user.Name}</td>
                            <td>{user.Ucn}</td>
                            <td>{user.Address}</td>
                            <td>{user.Country}</td>
                            <td>{user.City}</td>
                            <td>{user.PhoneNumber}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = FetchData.renderUsersTable(this.state.users);

        return (
            <div>
                <h1 id="tabelLabel" >Users</h1>
                {contents}
                <input type="button" value="Refresh" onClick={this.populateUsersData} />
            </div>
        );
    }

    async populateUsersData() {
        const response = await fetch('Users/GetAll');
        const data = await response.json();
        this.setState({ users: data });
    }
}
