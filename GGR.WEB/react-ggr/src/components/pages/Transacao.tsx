import { useEffect, useState, useRef } from "react";
import { toast } from "react-toastify";
import { SelectPessoa } from "../SelectPessoa";
import { SelectCategoria } from "../SelectCategoria";
import { GenericTable } from "../GenericTable"
import  {Column , TransacaoResponse, TransacaoRequest, PessoaResponse, CategoriaResponse} from "../../interface/types";
import { TransacaoService } from "../../api/TransacaoService";

const tipoTransacao: Record<number, string> = { 0: "Ambos", 1: "Receita", 2: "Despesa" };

export default function Transacao() {
    const [transacoes, setTransacoes] = useState([]);
    const [loading, setLoading] = useState(true);
    const [showModal, setShowModal] = useState(false); 
    const pessoaRef = useRef<PessoaResponse | null>(null);
    const categoriaRef = useRef<CategoriaResponse>(null);
    const valorRef = useRef(null); 
    const tipoRef = useRef(null);

    useEffect(() => {
        carregarTransacoes();
    }, []);

    async function carregarTransacoes() {
        const response = await TransacaoService.list();  
        setTransacoes(response);
        setLoading(false);
    }
 
    function selecionarPessoa(pessoa: PessoaResponse) {
        pessoaRef.current = pessoa;
    } 

    function selecionarCategoria(categoria: CategoriaResponse) {
        categoriaRef.current = categoria;
    }
  
    async function salvarTransacao() {
        const pessoaId = pessoaRef.current?.id;
        const categoriaId = categoriaRef.current?.id;
        const valor = Number(valorRef.current);
        const tipo = Number(tipoRef.current);

        const transacao: TransacaoRequest = {
            pessoa: { id: pessoaId! },
            categoria: { id: categoriaId! },
            valor: valor,
            tipo: tipo
        }

        if (!pessoaId || !categoriaId || !valor || tipo === 0) {
            toast.warning("Preencha todos os campos obrigatórios ( * ).", {
                style: { background: "#ffc107", color: "#000" },
                position: "bottom-right",
            });
            
            return;
        }

        const response = await TransacaoService.create(transacao); 

        if (!response?.id) {
            const data: { error?: string } = response;
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

    const colunas: Column<TransacaoResponse>[] =
    [
        { header: "Nome", accessor: (row ) => row.pessoa?.nome ?? "" },
        { header: "Categoria", accessor: (row) => row.categoria?.descricao ?? ""}, 
        { header: "Valor", accessor: "valor"},
        { header: "Tipo", accessor: (row: TransacaoResponse) => tipoTransacao[row.tipo] ?? "-" }
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
                              <SelectPessoa onChange={(option) => { option?.value, option?.label }}/>
                          </div>
                          <div className="mb-3">
                              <label className="form-label">Categoria *</label>
                              <SelectCategoria onChange={(option) => { option?.value, option?.label }}/>
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
