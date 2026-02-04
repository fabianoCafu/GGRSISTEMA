import { useEffect, useState } from "react";
import { SaldoLiquidoPessoa } from "../../interface/types";
import { BASE_URLS } from '../../config/api.config';

export default function SaldoLiquidoPessoa() {

    const [saldoLiquidos, setSaldoLiquidos] = useState<SaldoLiquidoPessoa[]>([]);
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {  
        carregarSaldosLiquido();   
    },[]);

    async function carregarSaldosLiquido() { 
        try {
            const response = await fetch(`${BASE_URLS.Transacao}/Transacao/getnetbalanceperson`);
            const data = await response.json();
            setSaldoLiquidos(data);
        } catch (error) {
            console.error("Erro ao carregar saldos", error);
        } finally {
            setLoading(false);
        }
    }

    function renderSaldosTable() {
        return (
            <table className="table table-striped">
                <thead>
                    <tr>
                        <th>Nome</th>
                        <th className="text-end">Receita</th>
                        <th className="text-end">Despesa</th>
                        <th className="text-end">Saldo</th>
                    </tr>
                </thead>
                <tbody>
                    {saldoLiquidos && saldoLiquidos.length > 0 ? (
                        <>
                            {saldoLiquidos.map((saldo) => (
                            <tr key={saldo.pessoaId}>
                                <td>{saldo.nome}</td>
                                <td align="right">{saldo.receitas}</td>
                                <td align="right">{saldo.despesas}</td>
                                <td align="right">{saldo.saldo}</td>
                            </tr>
                            ))} 
                            <tr>
                                <td></td>
                                <td align="right">
                                    <label className="fw-bold">Total Receita:</label> 
                                    <span className="ms-2">{saldoLiquidos[0].totalReceitas}</span> 
                                </td>
                                <td align="right">
                                    <label className="fw-bold">Total Despesa:</label> 
                                    <span className="ms-2">{saldoLiquidos[0].totalDespesas}</span> 
                                </td>
                                <td align="right">
                                    <label className="fw-bold">Total Saldo Liquido:</label>
                                    <span className="ms-2">{saldoLiquidos[0].totalSaldo}</span> 
                                </td>
                            </tr>  
                        </>
                    ) : (
                    <tr>
                        <td colSpan={4} className="text-center">
                            Nenhum saldo encontrado
                        </td>
                    </tr>
                    )}
                </tbody>
            </table>
    );
  }

  return (
      <div>
          <div className="d-flex justify-content-between align-items-center mb-3">
              <h1>Saldo por Pessoa</h1>
          </div>
          {loading ? (
              <div className="position-fixed top-0 start-0 w-100 h-100 d-flex flex-column justify-content-center align-items-center bg-white bg-opacity-75">
                  <div className="spinner-border text-secondary" role="status"></div>
                  <span className="mt-2">Carregando Saldos...</span>
              </div>
          ) : (
            renderSaldosTable()
          )}
      </div>
  );
}
