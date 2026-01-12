import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import { SelectPessoa } from "../SelectPessoa";
import { SelectCategoria } from "../SelectCategoria";

const tipoTransacao = { 0: "Ambos", 1: "Receita", 2: "Despesa", };

export default function Transacao() {
    const [transacoes, setTransacoes] = useState([]);
    const [loading, setLoading] = useState(true);
    const [showModal, setShowModal] = useState(false);
    const [pessoaId, setPessoaId] = useState(null);
    const [categoriaId, setCategoriaId] = useState(null);
    const [valor, setValor] = useState("");
    const [tipo, setTipo] = useState(0);

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
        if (!opcao) 
        {
            return;
        } 
        setPessoaId(opcao.value);
    }

    function selecionarCategoria(opcao) {
        if (!opcao) 
        {
            return;
        }
        setCategoriaId(opcao.value);
    }

    async function salvarTransacao() {
        if (!pessoaId || !categoriaId || !valor) {
            toast.warning("Preencha todos os campos obrigatórios.", {
                style: { background: "#ffc107", color: "#000" },
                position: "bottom-right",
            });
            return;
        }

        const response = await fetch( "https://localhost:7070/api/v1/Transacao/create",
        {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                pessoaId,
                categoriaId,
                valor: Number(valor),
                tipo,
            }),
        }
    );

    if (!response.ok) {
        const data = await response.json();
        toast.warning(data?.error || "Erro ao salvar transação", {
            style: { background: "#ffc107", color: "#000" },
            position: "bottom-right",
        });
        return;
    }

    setShowModal(false);
    setPessoaId(null);
    setCategoriaId(null);
    setValor("");
    setTipo(0);

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
                              <input type="number" className="form-control" value={valor} onChange={(e) => setValor(e.target.value)}/>
                          </div>
                          <div className="mb-3">
                              <label className="form-label">Tipo *</label>
                              <select className="form-select" value={tipo} onChange={(e) => setTipo(Number(e.target.value))}>
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
