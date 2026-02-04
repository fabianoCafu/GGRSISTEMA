import AsyncSelect from "react-select/async";
import { SelectProps , OptionType, Pessoa } from "../interface/types";

export function SelectPessoa({ onChange }: SelectProps) {
    const loadOptions = async (
        inputValue: string
    ): Promise<OptionType[]> => {
        if (!inputValue || inputValue.length < 3) {
            return [];
        }

        try {
        const response = await fetch(
            `https://localhost:7070/api/v1/Pessoa/getbyname?nomePessoa=${encodeURIComponent(
            inputValue)}`
        );

        if (!response.ok) {
            throw new Error("Erro ao buscar pessoas");
        }

        const data: Pessoa[] = await response.json();

        return data.map((pessoa) => ({
            value: pessoa.id,
            label: pessoa.nome,
        }));
        } catch (error) {
            console.error("Erro no SelectPessoa:", error);
            return [];
        }
    };

    return (
        <AsyncSelect<OptionType, false>
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
