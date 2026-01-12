import AsyncSelect from "react-select/async";

export function SelectCategoria({ onChange }) {
    const loadOptions = async (inputValue) => {
        if (inputValue.length < 3 || !inputValue) {
            return [];
        }

        try {
                const response = await fetch(`https://localhost:7188/api/v1/Categoria/getbydescription?descricaCategoria=${encodeURIComponent(inputValue)}`);

                if (!response.ok) {
                    throw new Error("Erro ao buscar categorias");
                }

                const data = await response.json();
                return data.map(categoria => ({ value: categoria.id, label: categoria.descricao}));
        } catch (error) {
            console.error("Erro no SelectCategoria:", error);
            return [];
        }
    };

    return (
        <AsyncSelect
            cacheOptions
            defaultOptions
            loadOptions={loadOptions}
            onChange={onChange}
            placeholder="Descrição"
            isClearable
            noOptionsMessage={() => "Nenhuma Categoria encontrada!"}
            loadingMessage={() => "Buscando categorias..."}
        />
    );
}
