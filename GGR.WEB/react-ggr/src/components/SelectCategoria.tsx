import AsyncSelect from "react-select/async";
import  {SelectProps , OptionType, Categoria} from "../interface/types";

export function SelectCategoria({ onChange }: SelectProps) {
    const loadOptions = async (
        inputValue: string
    ): Promise<OptionType[]> => {
        if (!inputValue || inputValue.length < 3) {
            return [];
        }

        try {
            const response = await fetch(
                `https://localhost:7188/api/v1/Categoria/getbydescription?descricaCategoria=${encodeURIComponent(
                inputValue)}`
            );

            if (!response.ok) {
                throw new Error("Erro ao buscar categorias");
            }

            const data: Categoria[] = await response.json();

            return data.map((categoria) => ({
                value: categoria.id,
                label: categoria.descricao,
            }));
        } catch (error) {
            console.error("Erro no SelectCategoria:", error);
            return [];
        }
    };

  return (
    <AsyncSelect<OptionType, false>
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
