// import { Outlet } from 'react-router-dom';
// import { Menu } from './Menu';

// export default function Layout() {
//   return (
//     <>
//       <Menu />
//       <div className="container mt-3">
//         <Outlet />
//       </div>
//     </>
//   );
// }


 import React, { Component } from 'react';
 import { Container } from 'reactstrap';
 import { NavMenu } from './NavMenu';
 import { Outlet } from 'react-router-dom';

 export class Layout extends Component {
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
