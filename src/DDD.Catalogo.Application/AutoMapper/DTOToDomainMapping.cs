using AutoMapper;
using DDD.Catalogo.Application.DTO;
using DDD.Catalogo.Domain.Entities;

namespace DDD.Catalogo.Application.AutoMapper
{
    public class DTOToDomainMapping : Profile
    {
        public DTOToDomainMapping()
        {
            CreateMap<CategoriaDTO, Categoria>()
                .ConstructUsing(c => new Categoria(c.Nome, c.Codigo));

            CreateMap<ProdutoDTO, Produto>()
                .ConstructUsing(p => new Produto
                (
                    p.Nome,
                    p.Descricao,
                    p.Ativo,
                    p.Valor,
                    p.CategoriaId,
                    p.DataCadastro,
                    p.Imagem,
                    new Dimensoes(p.Altura, p.Largura, p.Profundidade))
                );
        }
    }
}