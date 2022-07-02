using AutoMapper;
using DDD.Catalogo.Application.DTO;
using DDD.Catalogo.Domain.Entities;

namespace DDD.Catalogo.Application.AutoMapper
{
    public class DomainToDTOMapping : Profile
    {
        public DomainToDTOMapping()
        {
            CreateMap<Categoria, CategoriaDTO>();

            CreateMap<Produto, ProdutoDTO>()
                .ForMember(d => d.Largura, o => o.MapFrom(s => s.Dimensoes.Largura))
                .ForMember(d => d.Altura, o => o.MapFrom(s => s.Dimensoes.Altura))
                .ForMember(d => d.Profundidade, o => o.MapFrom(s => s.Dimensoes.Profundidade));
        }
    }
}