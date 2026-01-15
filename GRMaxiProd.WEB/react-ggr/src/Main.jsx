//import React from 'react';
import { StrictMode } from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import 'bootstrap/dist/css/bootstrap.min.css';

const container = document.getElementById('root');
const root = ReactDOM.createRoot(container);

root.render( 
           <StrictMode>
               <BrowserRouter>   
                   <App />
               </BrowserRouter>  
           </StrictMode>     
);









