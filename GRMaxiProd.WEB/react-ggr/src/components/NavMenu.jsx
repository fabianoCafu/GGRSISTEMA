/* eslint-disable jsx-a11y/anchor-is-valid */
import React, { Component } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';

export class NavMenu extends Component {
    static displayName = NavMenu.name;

    constructor(props) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            collapsed: true
        };
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }

    render() {
        return (
            <header>
                <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
                    <NavbarBrand tag={Link} to="/">Sistema GGR</NavbarBrand>
                    <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                    <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
                        <ul className="navbar-nav flex-grow">
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
                            </NavItem> 
                            <li className="nav-item dropdown">
                                <a className="nav-link dropdown-toggle text-dark" href="#" id="transacoesDropdown" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                                    Pessoa
                                </a>
                                <ul className="dropdown-menu" aria-labelledby="transacoesDropdown">
                                    <li>
                                        <NavLink tag={Link} className="dropdown-item" to="/pessoa">Listar Pessoas</NavLink>
                                    </li>
                                    <li>
                                        <NavLink tag={Link} className="dropdown-item" to="/pessoa">Saldo Pessoa</NavLink>
                                    </li>
                                </ul>
                            </li> 
                             <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/categoria">Listar Categorias</NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink tag={Link} className="text-dark" to="/transacao">Transacoes</NavLink>
                            </NavItem>
                        </ul>
                    </Collapse>
                </Navbar>
            </header>
        );
    }
}
