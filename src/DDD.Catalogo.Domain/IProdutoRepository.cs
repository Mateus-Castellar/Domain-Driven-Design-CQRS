using DDD.Catalogo.Domain.Entities;
using DDD.Core.Data;

namespace DDD.Catalogo.Domain
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> ObterTodos();
        Task<Produto> ObterPorId(Guid id);
        Task<IEnumerable<Produto>> ObterPorCategoria(int codigo);
        Task<IEnumerable<Categoria>> ObterCategorias();

        //Produtos
        void Adicionar(Produto produto);
        void Atualizar(Produto produto);

        //Categorias
        void Adicionar(Categoria categoria);
        void Atualizar(Categoria categoria);
    }
}
