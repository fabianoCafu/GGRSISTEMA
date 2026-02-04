import { useEffect, useRef, useState } from "react";
import { toast } from "react-toastify";
import { GenericTable } from "../GenericTable"
import  {Column , CategoriaRequest, CategoriaResponse } from "../../interface/types";
import { CategoriaService } from "../../api/CategoriaService";

const finalidadeLabel: Record<number, string> = { 1: "Receita", 2: "Despesa", 3: "Ambas" };

export default function Categoria() {
    const [categorias, setCategorias] = useState([]);
    const [loading, setLoading] = useState(true);
    const [showModal, setShowModal] = useState(false);
    const descricaoRef = useRef<HTMLInputElement | null>(null);
    const finalidadeRef = useRef<HTMLSelectElement | null>(null);

    useEffect(() => {
        carregarCategorias();
    }, []);

    async function carregarCategorias() {
        const response = await CategoriaService.list(); 
        setCategorias(response);
        setLoading(false);
    }

    async function salvarCategoria() {
        const descricaoInput = descricaoRef.current;
        const finalidadeInput = Number(finalidadeRef.current?.value.trim());

        if ((!descricaoInput || descricaoInput.value.trim() === "")
            && (!finalidadeInput || finalidadeInput === 0)) {
            toast.warning("Preencha todos os campos obrigatórios ( * ).", {
                style: { background: "#ffc107", color: "#000" },
                position: "bottom-right",
            });

            return;
        }
        
        const categoria: CategoriaRequest = {
            descricao: descricaoInput!.value.trim(),
            finalidade: Number(finalidadeInput) 
        };
        
        if (!categoria.descricao || categoria.finalidade === 0 ) {
            toast.warning("Descrição e Finalidade precisam ser preenchidos!", {
                style: { background: "#ffc107", color: "#000" },
                position: "bottom-right",
            });
            
            return;
        }
         
        const response = await CategoriaService.create(categoria); 

        if (!response?.id) {
            const data: { error?: string } = response;
            toast.warning(data?.error || "Erro ao salvar Categoria", {
                style: { background: "#ffc107", color: "#000" },
                position: "bottom-right",
            });

            return;
        }

        setShowModal(false);
        descricaoRef.current = null;
        finalidadeRef.current = null;

        toast.success("Categoria cadastrada com Sucesso!", {
            style: { background: "#228B22", color: "#000000" },
            position: "bottom-right",
        });

        carregarCategorias();
    }

    const colunas: Column<CategoriaResponse>[] = [
        { header: "Descrição", accessor: "descricao" },
        { header: "Finalidade",
            accessor: (row) => finalidadeLabel[row.finalidade] ?? "—"
        }
    ];

    function renderTabela() {
        return (<GenericTable columns={colunas} data={categorias} />);
    }
    
    return (
        <div>
            <div className="mb-3"> 
                <h1>Lista Categorias</h1>
                <div className="d-flex justify-content-end">
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
