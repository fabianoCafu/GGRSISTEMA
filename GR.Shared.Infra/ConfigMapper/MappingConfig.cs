using AutoMapper;
using GR.Shared.Infra.DTO;
using GR.Shared.Infra.Model;

namespace GR.Shared.Infra.ConfigMapper
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<CategoriaDtoRequest, Categoria>().ReverseMap();
                config.CreateMap<CategoriaDtoResponse, Categoria>().ReverseMap();
                config.CreateMap<PessoaDtoRequest, Pessoa>().ReverseMap();
                config.CreateMap<PessoaDtoResponse, Pessoa>().ReverseMap();
                config.CreateMap<TransacaoDtoRequest, Transacao>().ReverseMap();
                config.CreateMap<TransacaoDtoResponse, Transacao>().ReverseMap();
            });
        }
    }
}
