import AsyncSelect from "react-select/async";

export function SelectPessoa({ onChange }) {
    const loadOptions = async (inputValue) => {
        if (inputValue.length < 3 || !inputValue) {
            return [];
        }

        try {
                const response = await fetch(`https://localhost:7070/api/v1/Pessoa/getbyname?nomePessoa=${encodeURIComponent(inputValue)}`);

                if (!response.ok) {
                    throw new Error("Erro ao buscar pessoas");
                }

                const data = await response.json();
                return data.map(pessoa => ({ value: pessoa.id, label: pessoa.nome}));
        } catch (error) {
            console.error("Erro no SelectPessoa:", error);
            return [];
        }
    };

    return (
        <AsyncSelect
            cacheOptions
            defaultOptions
            loadOptions={loadOptions}
            onChange={onChange}
            placeholder="Nome"
            isClearable
            noOptionsMessage={() => "Nenhuma Pessoa encontrada!"}
            loadingMessage={() => "Buscando pessoas..."}
        />
    );
}
