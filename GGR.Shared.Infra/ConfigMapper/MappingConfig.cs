using AutoMapper;
using GGR.Shared.Infra.DTO;
using GGR.Shared.Infra.Model;

namespace GGR.Shared.Infra.ConfigMapper
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
