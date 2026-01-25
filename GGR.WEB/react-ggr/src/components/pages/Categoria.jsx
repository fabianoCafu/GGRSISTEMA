import { useEffect, useRef, useState } from "react";
import { toast } from "react-toastify";

const finalidadeLabel = { 1: "Receita", 2: "Despesa", 3: "Ambas"};

export default function Categoria() {
    const [categorias, setCategorias] = useState([]);
    const [loading, setLoading] = useState(true);
    const [showModal, setShowModal] = useState(false);
    const descricaoRef = useRef(null);
    const finalidadeRef = useRef(null);

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
        const descricao = descricaoRef.current?.value.trim();
        const finalidade = Number(finalidadeRef.current?.value);

        if (!descricao || finalidade === 0) {
            toast.warning("Preencha todos os campos obrigatórios ( * ).", {
                style: { background: "#ffc107", color: "#000" },
                position: "bottom-right",
            });
            return;
        }

        const response = await fetch('https://localhost:7188/api/v1/Categoria/create',
        {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                descricao: descricao,
                finalidade: finalidade
            }),
        });

        if (!response.ok) {
            const data = await response.json();
            toast.warning(data?.error || "Erro ao salvar categoria", {
                style: { background: "#ffc107", color: "#000" },
                position: "bottom-right",
            });
            return;
        }

        setShowModal(false);
        descricaoRef.current.value = "";
        finalidadeRef.current.value = 1;

        toast.success("Categoria cadastrada com Sucesso!", {
            style: { background: "#228B22", color: "#000000" },
            position: "bottom-right",
        });

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
                                <input type="text" className="form-control" ref={descricaoRef}/>
                            </div>
                            <div className="mb-3">
                                <label className="form-label">Finalidade *</label>
                                <select className="form-select" ref={finalidadeRef} >
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
