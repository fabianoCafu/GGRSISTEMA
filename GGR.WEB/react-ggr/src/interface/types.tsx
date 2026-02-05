import { SingleValue } from "react-select";

export interface Column<T> {
    header: string;
    accessor?: keyof T | ((row: T) => React.ReactNode);
    render?: (value: any, row: T) => React.ReactNode;
}

export interface PessoaResponse {
    id: string;
    nome: string;
    idade: number;
}

export interface PessoaRequest { 
    nome: string;
    idade: number;
}

export interface CategoriaResponse {
    id: string;
    descricao: string;
    finalidade: number;
}

export interface CategoriaRequest {
    descricao: string;
    finalidade: number;
}

export interface TransacaoRequest { 
    pessoaId: string;    //{ id: string }
    categoriaId: string; //{ id: string }
    valor: number
    tipo: number
}

export interface TransacaoResponse {
    id: number;
    pessoa: {
        id: string
        nome: string
    }
    categoria: {
        id: string
        descricao: string
    }
    valor: number
    tipo: number
}

export interface SaldoLiquidoCategoria {
    categoriaId: string;
    nome: string;
    receitas: number;
    despesas: number;
    saldo: number;
    totalReceitas: number;
    totalDespesas: number;
    totalSaldo: number;
}

export interface SaldoLiquidoPessoa {
    pessoaId: string;
    nome: string;
    receitas: number;
    despesas: number;
    saldo: number;
    totalReceitas: number;
    totalDespesas: number;
    totalSaldo: number;
}

export interface Categoria {
  id: string;
  descricao: string;
}

export interface Pessoa {
  id: string;
  nome: string;
}

export interface OptionType {
  value: string;
  label: string;
}

export interface SelectProps {
  onChange: (option: SingleValue<OptionType>) => void;
}

