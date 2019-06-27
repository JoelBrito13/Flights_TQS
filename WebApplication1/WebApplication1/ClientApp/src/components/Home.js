import React, { Component } from 'react'
import axios from '../axios';

import Button from 'react-bootstrap/Button'
const { API_KEY } = process.env
const API_URL = 'https://localhost:44310/api/Searchers/';

export class Home extends Component {
    tate = {
        query: '',
        results: []
    }

    getInfo = () => {
        axios.get(`${API_URL}?api_key=${API_KEY}&prefix=${this.state.query}&limit=7`)
            .then(({ data }) => {
                this.setState({
                    results: data.data // MusicGraph returns an object named data, 
                    // as does axios. So... data.data                             
                })
            })
    }

    handleInputChange = () => {
        this.setState({
            query: this.search.value
        }, () => {
            if (this.state.query && this.state.query.length > 1) {
                if (this.state.query.length % 2 === 0) {
                    this.getInfo()
                }
            }
        })
    }

    render() {
        return (
            <div>

                return (
            <form>
                    <input
                        placeholder="Departure Airport"
                        ref={input => this.search = input}
                        onChange={this.handleInputChange}
                    />
                    <p>{this.state.query}</p>
                </form>
                <form>
                    <input
                        placeholder="Arrive Airport"
                        ref={input => this.search = input}
                        onChange={this.handleInputChange}
                    />
                    <p>{this.state.query}</p>
                </form>
                <Button variant="primary" size="lg">
                    Search
                </Button>
            </div>
        );
    };
}
export default Home




