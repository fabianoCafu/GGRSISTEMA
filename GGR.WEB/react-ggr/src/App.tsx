import { BrowserRouter, Routes, Route } from "react-router-dom";
import { Layout } from './Layout';
import { ToastContainer } from "react-toastify";
import Home  from './components/pages/Home';
import Pessoa from "./components/pages/Pessoa";
import Categoria  from './components/pages/Categoria';
import Transacao  from './components/pages/Transacao';
import SaldoLiquidoPessoa  from "./components/pages/SaldoLiquidoPessoa";
import SaldoLiquidoCategoria  from "./components/pages/SaldoLiquidoCategoria";
import "react-toastify/dist/ReactToastify.css";
import './custom.css';

function App() { 
    return ( 
            <>
                <Routes>     
                    <Route element={<Layout />}> 
                        <Route path="/" element={<Home />} />
                        <Route path="/pessoa" element={<Pessoa />} />
                        <Route path="/categoria" element={<Categoria />} />
                        <Route path="/transacao" element={<Transacao />} /> 
                        <Route path="/saldo-liquido-pessoa/" element={<SaldoLiquidoPessoa />} /> 
                        <Route path="/saldo-liquido-categoria/" element={<SaldoLiquidoCategoria />} />
                    </Route> 
                </Routes>
                <ToastContainer position="top-right" autoClose={3000} />
            </> 
    )
};

export default App;
