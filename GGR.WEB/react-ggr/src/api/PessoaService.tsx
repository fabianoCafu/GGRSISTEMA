import { PessoaRequest } from '../interface/types';
import { BASE_URLS } from '../config/api.config';

export const PessoaService = {
    async list() { 
        const response = await fetch(`${BASE_URLS.Pessoa}/Pessoa/list`, { method: "GET" });
        return response.json();
    },

    async create(pessoa: PessoaRequest) { 
        const response = await fetch(`${BASE_URLS.Pessoa}/Pessoa/create`,
        {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(pessoa),
        });
        
        return response.json();
    },

    async delete(id: number | string) {
        await fetch(`${BASE_URLS.Pessoa}/Pessoa/delete/${id}`, { method: "DELETE" });
    }
};
