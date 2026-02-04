import React, { Component, PropsWithChildren } from "react";
import { Container } from "reactstrap";
import { Outlet } from "react-router-dom";
import NavMenu from "./NavMenu";

export class Layout extends Component<PropsWithChildren> {
    static displayName = Layout.name;

    render() {
        return (
            <div>
                <NavMenu />
                <Container>
                    <Outlet />
                    {this.props.children}
                </Container>
            </div>
    );
  }
}
