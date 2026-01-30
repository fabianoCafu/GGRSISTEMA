
export interface Column<T> {
    header: string;
    accessor?: keyof T | ((row: T) => React.ReactNode);
    render?: (value: any, row: T) => React.ReactNode;
}

export interface PessoaResponse {
    id: number;
    nome: string;
    idade: number;
}

export interface PessoaRequest { 
    nome: string;
    idade: number;
}


export interface Categoria {
    id: number;
    descricao: string;
    finalidade: number;
}

export interface Transacao {
    id: number;
    pessoa: {
        id: number
        nome: string
    }
    categoria: {
        id: number
        descricao: string
    }
    valor: number
    tipo: number
}
