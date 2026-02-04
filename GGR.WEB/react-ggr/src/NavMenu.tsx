/* eslint-disable jsx-a11y/anchor-is-valid */
import React, { useState } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';

const NavMenu = () => {
    const [collapsed, setCollapsed] = useState(true);
    const toggleNavbar = () => {
        setCollapsed(!collapsed);
    };

    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm bg-white border-bottom box-shadow mb-3" container light>
            <NavbarBrand tag={Link} to="/">
                GGR - Sistema
            </NavbarBrand>
            <NavbarToggler onClick={toggleNavbar} className="me-2" />
                <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar >
                    <ul className="navbar-nav flex-grow">
                        <NavItem>
                            <NavLink tag={Link} className="text-dark" to="/">
                                Home
                            </NavLink>
                        </NavItem>
                        <li className="nav-item dropdown">
                            <a className="nav-link dropdown-toggle text-dark" href="#" id="transacoesDropdown" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                                Pessoa
                            </a>
                            <ul className="dropdown-menu" aria-labelledby="transacoesDropdown">
                                <li>
                                    <NavLink tag={Link} className="dropdown-item" to="/pessoa">
                                        Lista Pessoas
                                    </NavLink>
                                </li>
                                <li>
                                    <NavLink tag={Link} className="dropdown-item" to="/saldo-liquido-pessoa">
                                        Saldo Liquído Pessoas
                                    </NavLink>
                                </li>
                                 <li>
                                    <NavLink tag={Link} className="dropdown-item" to="/saldo-liquido-categoria">
                                        Saldo Liquído Categorias
                                    </NavLink>
                                </li>
                            </ul>
                        </li>
                        <NavItem>
                            <NavLink tag={Link} className="text-dark" to="/transacao">
                                Transações
                            </NavLink>
                        </NavItem>
                        <NavItem>
                            <NavLink tag={Link} className="text-dark" to="/categoria">
                                Listar Categorias
                            </NavLink>
                        </NavItem> 
                    </ul>
                </Collapse>
            </Navbar>
    </header>
  );
};

export default NavMenu;


