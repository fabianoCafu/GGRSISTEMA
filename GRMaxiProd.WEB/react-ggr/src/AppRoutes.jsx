import { Pessoa } from "./components/pages/Pessoa";
import { Categoria } from "./components/pages/Categoria";
import { Transacao } from "./components/pages/Transacao";
import { SaldoLiquidoPessoa } from "./components/pages/Transacao";
import { Home } from "./Home";

const AppRoutes = [
    {
        index: true,
        element: <Home />
    },
    {
        path: '/pessoas',
        element: <Pessoa />
    },
    {
        path: '/categorias',
        element: <Categoria />
    },
    {
        path: '/transacoes',
        element: <Transacao />
    },
    {
        path: '/saldo-liquido-pessoa',
        element: <SaldoLiquidoPessoa />
    },
    {
        path: '/saldo-liquido-categoria',
        element: <SaldoLiquidoCategoria />
    },
];

export default AppRoutes;
