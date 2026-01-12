import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

export default function SaldoLiquido() {
    const { idPessoa } = useParams();
    const [saldoLiquidos, setSaldoLiquidos] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => { 
        if (idPessoa) {
            carregarSaldosLiquido(idPessoa);
        }
    }, [idPessoa]);

    async function carregarSaldosLiquido(idPessoa){ 
        try {
            const response = await fetch(`https://localhost:7070/api/v1/Transacao/getnetbalance?idPessoa=${idPessoa}`);
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
                        <th>Receita</th>
                        <th>Despesa</th>
                        <th>Saldo</th>
                    </tr>
                </thead>
                <tbody>
                {saldoLiquidos && saldoLiquidos.length > 0 ? (
                saldoLiquidos.map((saldo) => (
                    <tr key={saldo.pessoaId}>
                        <td>{saldo.nome}</td>
                        <td>{saldo.totalReceitas}</td>
                        <td>{saldo.totalDespesas}</td>
                        <td>{saldo.saldo}</td>
                    </tr>
                ))) : (
                   <tr>
                       <td colSpan="4" className="text-center">
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
              <h1>Lista Saldo Líquido</h1>
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
