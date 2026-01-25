import { useEffect, useState, useRef } from "react";
import { toast } from "react-toastify";
import { SelectPessoa } from "../SelectPessoa";
import { SelectCategoria } from "../SelectCategoria";

const tipoTransacao = { 0: "Ambos", 1: "Receita", 2: "Despesa", };

export default function Transacao() {
    const [transacoes, setTransacoes] = useState([]);
    const [loading, setLoading] = useState(true);
    const [showModal, setShowModal] = useState(false); 
    const pessoaRef = useRef(null);
    const categoriaRef = useRef(null);
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

    function selecionarPessoa(opcao) {
        pessoaRef.current = opcao; 
    }

    function selecionarCategoria(opcao) {
        categoriaRef.current = opcao;
    }
  
    async function salvarTransacao() {
        const pessoa = pessoaRef.current;
        const categoria = categoriaRef.current;
        const valor = valorRef.current?.value;
        const tipo = Number(tipoRef.current?.value);

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
                pessoaId: pessoa.value,
                categoriaId: categoria.value,
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
        valorRef.current.value = "";
        tipoRef.current.value = 0;
        pessoaRef.current = null;
        categoriaRef.current = null;

        toast.success("Transação cadastrada com Sucesso!", {
            style: { background: "#228B22", color: "#000000" },
            position: "bottom-right",
        });

        carregarTransacoes();
    }

  function renderTabela() {
      return (
          <table className="table table-striped">
              <thead>
                  <tr>
                      <th>Nome</th>
                      <th>Categoria</th>
                      <th>Valor</th>
                      <th>Tipo</th>
                  </tr>
              </thead>
              <tbody>
                  {transacoes.map((t) => (
                  <tr key={t.id}>
                      <td>{t.pessoa.nome}</td>
                      <td>{t.categoria.descricao}</td>
                      <td>{t.valor}</td>
                      <td>{tipoTransacao[t.tipo] ?? "Desconhecida"}</td>
                  </tr>
                  ))}
              </tbody>
          </table>);
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
