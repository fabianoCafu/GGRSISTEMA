import { useEffect, useRef, useState } from "react";
import Swal from "sweetalert2";
import { toast } from "react-toastify";

export default function Pessoa() {
    const [pessoas, setPessoas] = useState([]);
    const [loading, setLoading] = useState(true);
    const [showModal, setShowModal] = useState(false);
    const nomeRef = useRef(null);
    const idadeRef = useRef(null);

    useEffect(() => {
       carregarPessoas();
    }, []);

    async function carregarPessoas() {
        const response = await fetch("https://localhost:7143/api/v1/Pessoa/list");
        const data = await response.json();
        setPessoas(data);
        setLoading(false);
    }

    async function salvarPessoa() {
        const nome = nomeRef.current;
        const idade = idadeRef.current;
        
        if (!nome || !idade) {
            toast.warning("Preencha todos os campos obrigatórios ( * ).", {
                style: { background: "#ffc107", color: "#000" },
                position: "bottom-right",
            });
            return;
        }

        const response = await fetch("https://localhost:7143/api/v1/Pessoa/create",
        {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                nome: nome.value,
                idade: idade.value, 
            }),
        });

        if (!response.ok) {
            const data = await response.json();
            toast.warning(data?.error || "Erro ao salvar Pessoa", {
                style: { background: "#ffc107", color: "#000" },
                position: "bottom-right",
            });
            return;
        }

        setShowModal(false);
        nomeRef.current.value = "";
        idadeRef.current.value = "";
        
        toast.success("Pessoa cadastrada com Sucesso!", {
            style: { background: "#228B22", color: "#000000" },
            position: "bottom-right",
        });

        carregarPessoas();
    }

    async function removerPessoa(pessoa) {
        const result = await Swal.fire({
            title: "Tem certeza?",
            text: `Remover ${pessoa.nome}?`,
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#d39e00",
            cancelButtonColor: "#6c757d",
            confirmButtonText: "Remover",
            cancelButtonText: "Cancelar",
        });

        if (!result.isConfirmed) {
            return;
        }

        await fetch(`https://localhost:7143/api/v1/Pessoa/delete/${pessoa.id}`,{ method: "DELETE" });

        carregarPessoas();
    }

    function renderTabela() {
        return (
            <table className="table table-striped">
                <thead>
                    <tr>
                        <th>Nome</th>
                        <th>Idade</th>
                        <th className="text-center">Ação</th>
                    </tr>
                </thead>
                <tbody>
                    {pessoas.map((pessoa) => (
                    <tr key={pessoa.id}>
                        <td>{pessoa.nome}</td>
                        <td>{pessoa.idade}</td>
                        <td className="text-center">
                            <div className="d-flex justify-content-center gap-2"> 
                                <button className="btn btn-danger btn-sm" style={{ padding: '3px 8px', fontSize: '13px' }} onClick={() => removerPessoa(pessoa)}>
                                    Remover
                                </button>
                            </div>
                        </td>
                    </tr>))}
                </tbody>
            </table>
        );
    }

  return (
      <div>
          <div className="mb-3"> 
              <h1>Lista Pessoas</h1>
              <div className="d-flex justify-content-end">
                  <button className="btn btn-warning text-end" style={{ padding: '3px 8px', fontSize: '13px' }} onClick={() => setShowModal(true)}>
                      Inserir Pessoa
                  </button>
              </div>
          </div>

          {loading ? (<div className="text-center">
                          <div className="spinner-border text-secondary" />
                          <div>Carregando Pessoas...</div>
                      </div>
          ) : ( renderTabela())}

          {showModal && (
              <div className="modal fade show d-block">
                  <div className="modal-dialog">
                      <div className="modal-content">
                          <div className="modal-header">
                              <h5 className="modal-title">Nova Pessoa</h5>
                              <button className="btn-close" onClick={() => setShowModal(false)} />
                          </div>
                          <div className="modal-body">
                              <div className="mb-3">
                                  <label className="form-label">Nome *</label>
                                  <input className="form-control mb-2" placeholder="Nome" ref={nomeRef}/>
                              </div>
                              <div className="mb-3">
                                  <label className="form-label">Idade *</label>
                                  <input type="number" className="form-control" placeholder="Idade" ref={idadeRef} />
                              </div>  
                          </div>
                          <div className="modal-footer">
                              <button className="btn btn-warning" onClick={salvarPessoa}>
                                  Salvar
                              </button>
                              <button className="btn btn-secondary" onClick={() => setShowModal(false)} >
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
