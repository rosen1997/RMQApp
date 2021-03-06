import React, { Component } from 'react';

export class Register extends Component {


    constructor(props) {
        super(props);
        this.state = {
            countries: [],
            cities: [],
            gender: null,
            country: null, city: null
        };
        this.onSubmit = this.onSubmit.bind(this);
        this.populateCountries = this.populateCountries.bind(this);
        this.onChangeGender = this.onChangeGender.bind(this);
        this.onChangeCountry = this.onChangeCountry.bind(this);
        this.onChangeCity = this.onChangeCity.bind(this);
    }

    componentDidMount() {
        this.populateCountries();
    }



    onSubmit(e) {
        var name = document.getElementById('name').value;
        var lastName = document.getElementById('ucn').value;
        var address = document.getElementById('address').value;
        var phoneNumber = document.getElementById('pN').value;
        var gender = (this.state.gender === 'true');

        const user = { Name: name, Ucn: lastName, Address: address, PhoneNumber: phoneNumber, Gender: gender, Country: this.state.country, City: this.state.city };

        e.preventDefault();
        console.log(JSON.stringify(user));
        fetch('Users/Create', {

            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user)

        });
    }

    onChangeGender(event) {
        this.setState({ gender: event.target.value });
    }

    onChangeCountry(event) {
        let obj = JSON.parse(event.target.value);
        this.setState({
            cities: obj.cities,
            country: obj.country
        });
    }

    onChangeCity(event) {
        this.setState({
            city: event.target.value,
        });
    }

    async populateCountries() {
        const response = await fetch('https://countriesnow.space/api/v0.1/countries');
        const data = await response.json();
        this.setState({ countries: data.data });
    }

    render() {
        return (
            <div>
                <h1>Sing Up</h1>
                <div className="form-wrapper">
                    <form className="forms-lr" onSubmit={this.onSubmit}>
                        <label>
                            Name:
                            <input type="text" id="name" />
                        </label>
                        <label>
                            UCN:
                            <input type="text" id="ucn" />
                        </label>
                        <label>
                            Address:
                            <input type="text" id="address" />
                        </label>
                        <label>
                            Gender:
                            <div onChange={this.onChangeGender}>
                                <input type="radio" value={true} name="gender" id="gender" /> Male
                                <input type="radio" value={false} name="gender" id="gender" /> Female
                            </div>
                        </label>
                        <label>
                            Phone Number:
                            <input type="tel" id="pN" />
                        </label>
                        <label>
                            Country:
                            <select id="country" onChange={this.onChangeCountry}>
                                <option>--Select Country--</option>
                                {this.state.countries.map((country) => <option value={JSON.stringify(country)}>{country.country}</option>)}
                            </select>
                        </label>
                        <label>
                            City:
                            <select id="city" onChange={this.onChangeCity}>
                                <option>--Select City--</option>
                                {this.state.cities.map((city) => <option value={city}>{city}</option>)}
                            </select>
                        </label>
                        <input className="submit-btn" type="submit" value="Sing Up" />
                    </form>
                </div>
            </div>
        );
    }
}