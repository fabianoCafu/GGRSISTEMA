import { TransacaoRequest } from '../interface/types';
import { BASE_URLS } from '../config/api.config';

export const TransacaoService = {
    async list() { 
        const response = await fetch(`${BASE_URLS.Transacao}/Transacao/list`, { method: "GET" });
        return response.json();
    },
    
    async create(transacao: TransacaoRequest) { 
        const response = await fetch(`${BASE_URLS.Transacao}/Transacao/create`,
        {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(transacao),
        });

        return response.json();
    }
};
