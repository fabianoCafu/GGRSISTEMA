import React, { useEffect, useState } from "react";
import  {SaldoLiquidoCategoria} from "../../interface/types";


export default function SaldoLiquidoCategoria() {
    const [saldoLiquidos, setSaldoLiquidos] = useState<SaldoLiquidoCategoria[]>([]);
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {  
        carregarSaldosLiquido();   
    }, []);

    async function carregarSaldosLiquido(): Promise<void> { 
        try {
            const response = await fetch("https://localhost:7070/api/v1/Transacao/getnetbalancecategory");
            const data: SaldoLiquidoCategoria[] = await response.json();
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
                        <th>Descrição</th>
                        <th className="text-end">Receita</th>
                        <th className="text-end">Despesa</th>
                        <th className="text-end">Saldo</th>
                    </tr>
                </thead>
                <tbody>
                    {saldoLiquidos.length > 0 ? (
                        <>
                            {saldoLiquidos.map((saldo) => (
                                <tr key={saldo.categoriaId}>
                                    <td>{saldo.nome}</td>
                                    <td className="text-end">{saldo.receitas}</td>
                                    <td className="text-end">{saldo.despesas}</td>
                                    <td className="text-end">{saldo.saldo}</td>
                                </tr>
                            ))}

                            <tr>
                                <td></td>
                                <td className="text-end">
                                    <strong>Total Receita:</strong>
                                    <span className="ms-2">
                                        {saldoLiquidos[0].totalReceitas}
                                    </span>
                                </td>
                                <td className="text-end">
                                    <strong>Total Despesa:</strong>
                                    <span className="ms-2">
                                        {saldoLiquidos[0].totalDespesas}
                                    </span>
                                </td>
                                <td className="text-end">
                                    <strong>Total Saldo Líquido:</strong>
                                    <span className="ms-2">
                                        {saldoLiquidos[0].totalSaldo}
                                    </span>
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
                <h1>Saldo por Categoria</h1>
            </div>

            {loading ? (
                <div className="position-fixed top-0 start-0 w-100 h-100 d-flex flex-column justify-content-center align-items-center bg-white bg-opacity-75">
                    <div className="spinner-border text-secondary" role="status" />
                    <span className="mt-2">Carregando Saldos...</span>
                </div>
            ) : (
                renderSaldosTable()
            )}
        </div>
    );
}
