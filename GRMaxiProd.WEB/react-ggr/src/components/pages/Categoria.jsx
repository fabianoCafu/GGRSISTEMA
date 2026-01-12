import { useEffect, useState } from "react";
import { toast } from "react-toastify";

const finalidadeLabel = { 1: "Receita", 2: "Despesa", 3: "Ambas"};

export default function Categoria() {
    const [categorias, setCategorias] = useState([]);
    const [loading, setLoading] = useState(true);
    const [showModal, setShowModal] = useState(false);
    const [descricao, setDescricao] = useState("");
    const [finalidade, setFinalidade] = useState(1);

    useEffect(() => {
        carregarCategorias();
    }, []);

    async function carregarCategorias() {
        const response = await fetch("https://localhost:7188/api/v1/Categoria/list");
        const data = await response.json();
        setCategorias(data);
        setLoading(false);
    }

    async function salvarCategoria() {
        if (!descricao.trim() || finalidade === undefined) {
            toast.warning("Preencha todos os campos obrigatórios.", {
                style: { background: "#ffc107", color: "#000" },
                position: "bottom-right",
            });
            return;
        }

        await fetch("https://localhost:7188/api/v1/Categoria/create", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ descricao, finalidade }),
        });

        setShowModal(false);
        setDescricao("");
        setFinalidade(1);
        carregarCategorias();
    }

    function renderTabela() {
        return (
            <table className="table table-striped">
                <thead>
                    <tr>
                    <th>Descrição</th>
                    <th className="text-center">Finalidade</th>
                    </tr>
                </thead>
                <tbody>
                    {categorias.map((c) => (
                    <tr key={c.id}>
                        <td>{c.descricao}</td>
                        <td className="text-center">
                        {finalidadeLabel[c.finalidade] ?? "Desconhecida"}
                        </td>
                    </tr>))}
                </tbody>
            </table>
        );
    }

    return (
        <div>
            <div className="mb-3"> 
                <h1>Lista Categorias</h1>
                <div class="d-flex justify-content-end">
                    <button className="btn btn-warning text-end" style={{ padding: '3px 8px', fontSize: '13px' }} onClick={() => setShowModal(true)}>
                        Inserir Categoria
                    </button>
                </div>
            </div>
            
            {loading ? (<div className="text-center">
                            <div className="spinner-border text-secondary" />
                            <div>Carregando Categorias...</div>
                        </div>
            ) : ( renderTabela())}

            {showModal && (
            <div className="modal fade show d-block">
                <div className="modal-dialog">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title">Nova Categoria</h5>
                            <button className="btn-close" onClick={() => setShowModal(false)}/>
                        </div>
                        <div className="modal-body">
                            <div className="mb-3">
                                <label className="form-label">Descrição *</label>
                                <input type="text" className="form-control" value={descricao} onChange={(e) => setDescricao(e.target.value)}/>
                            </div>
                            <div className="mb-3">
                                <label className="form-label">Finalidade *</label>
                                <select className="form-select" value={finalidade} onChange={(e) => setFinalidade(Number(e.target.value))}>
                                    <option value={0}>- Selecione -</option>
                                    <option value={1}>Receita</option>
                                    <option value={2}>Despesa</option>
                                    <option value={3}>Ambas</option>
                                </select>
                            </div>
                        </div>
                        <div className="modal-footer">
                            <button className="btn btn-warning" onClick={salvarCategoria}>
                                Salvar
                            </button>
                            <button className="btn btn-secondary" onClick={() => setShowModal(false)}>
                                Cancelar
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        )}
        </div>
    );
}
