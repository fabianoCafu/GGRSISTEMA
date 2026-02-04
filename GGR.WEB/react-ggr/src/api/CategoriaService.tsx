import { CategoriaRequest } from '../interface/types';
import { BASE_URLS } from '../config/api.config';

export const CategoriaService = {
    async list() { 
        const response = await fetch(`${BASE_URLS.Categoria}/Categoria/list`,{ method: "GET" });
        return response.json();
    },

    async create(categoria: CategoriaRequest) { 
        const response = await fetch(`${BASE_URLS.Categoria}/Categoria/create`,
        {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(categoria),
        });

        return response.json();
    }
};
