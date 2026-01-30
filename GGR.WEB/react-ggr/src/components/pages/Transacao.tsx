import { useEffect, useState, useRef } from "react";
import { toast } from "react-toastify";
import { SelectPessoa } from "../SelectPessoa";
import { SelectCategoria } from "../SelectCategoria";
import { GenericTable } from "../GenericTable"
import  {Column , Pessoa, Categoria, Transacao} from "../interface/types";

const tipoTransacao: Record<number, string> = { 0: "Ambos", 1: "Receita", 2: "Despesa" };

export default function Transacao() {
    const [transacoes, setTransacoes] = useState([]);
    const [loading, setLoading] = useState(true);
    const [showModal, setShowModal] = useState(false); 
    const pessoaRef = useRef<Pessoa | null>(null);
    const categoriaRef = useRef<Categoria>(null);
    const valorRef = useRef(null); 
    const tipoRef = useRef(null);

    useEffect(() => {
        carregarTransacoes();
    }, []);

    async function carregarTransacoes() {
        const response = await fetch("https://localhost:7070/api/v1/Transacao/list");
        const data = await response.json();
        setTransacoes(data);
        setLoading(false);
    }
 
    function selecionarPessoa(pessoa: Pessoa) {
        pessoaRef.current = pessoa;
    } 

    function selecionarCategoria(categoria: Categoria) {
        categoriaRef.current = categoria;
    }
  
    async function salvarTransacao() {
        const pessoa = pessoaRef.current?.id;
        const categoria = categoriaRef.current?.id;
        const valor = valorRef.current;
        const tipo = Number(tipoRef.current);

        if (!pessoa || !categoria || !valor || tipo === 0) {
            toast.warning("Preencha todos os campos obrigatórios ( * ).", {
                style: { background: "#ffc107", color: "#000" },
                position: "bottom-right",
            });
            return;
        }

        const response = await fetch("https://localhost:7070/api/v1/Transacao/create",
        {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                pessoaId: pessoa,
                categoriaId: categoria,
                valor: Number(valor),
                tipo,
            }),
        });

        if (!response.ok) {
            const data = await response.json();
            toast.warning(data?.error || "Erro ao salvar transação", {
                style: { background: "#ffc107", color: "#000" },
                position: "bottom-right",
            });
            return;
        }

        setShowModal(false);
        valorRef.current = null;
        tipoRef.current = null;
        pessoaRef.current = null;
        categoriaRef.current = null;

        toast.success("Transação cadastrada com Sucesso!", {
            style: { background: "#228B22", color: "#000000" },
            position: "bottom-right",
        });

        carregarTransacoes();
    }

    const colunas: Column<Transacao>[] =
    [
        { header: "Nome", accessor: (row ) => row.pessoa?.nome ?? "" },
        { header: "Categoria", accessor: (row) => row.categoria?.descricao ?? ""}, 
        { header: "Valor", accessor: "valor"},
        { header: "Tipo", accessor: (row: Transacao) => tipoTransacao[row.tipo] ?? "-" }
   ];

    function renderTabela() {
        return (<GenericTable columns={colunas} data={transacoes} />);
    }

  return (
      <div>
          <div className="mb-3"> 
              <h1>Lista Transações</h1>
              <div className="d-flex justify-content-end">
                  <button className="btn btn-warning text-end" style={{ padding: '3px 8px', fontSize: '13px' }} onClick={() => setShowModal(true)}>
                      Inserir Transação
                  </button>
              </div>
          </div> 

          {loading ? (<div className="text-center">
                          <div className="spinner-border text-secondary" />
                          <div>Carregando Transações...</div>
                      </div>) : ( renderTabela() )}

          {showModal && (
          <div className="modal fade show d-block">
              <div className="modal-dialog">
                  <div className="modal-content">
                      <div className="modal-header">
                          <h5 className="modal-title">Nova Transação</h5>
                          <button className="btn-close" onClick={() => setShowModal(false)}/>
                      </div>

                      <div className="modal-body">
                          <div className="mb-3">
                              <label className="form-label">Pessoa *</label>
                              <SelectPessoa onChange={selecionarPessoa} /> 
                          </div>
                          <div className="mb-3">
                              <label className="form-label">Categoria *</label>
                               <SelectCategoria onChange={selecionarCategoria} />
                          </div>
                          <div className="mb-3">
                              <label className="form-label">Valor *</label>
                              <input type="number" className="form-control" ref={valorRef} />
                          </div>
                          <div className="mb-3">
                              <label className="form-label">Tipo *</label>
                              <select className="form-select" ref={tipoRef} >
                                  <option value={0}>- Selecione -</option>
                                  <option value={1}>Receita</option>
                                  <option value={2}>Despesa</option>
                              </select>
                          </div>
                      </div>
                      <div className="modal-footer">
                          <button className="btn btn-warning" onClick={salvarTransacao}>
                              Salvar
                          </button>
                          <button className="btn btn-secondary" onClick={() => setShowModal(false)}>
                              Cancelar
                          </button>
                      </div>
                  </div>
              </div>
          </div>)}
      </div>
  );
}
