using DDD.Catalogo.Domain;
using DDD.Catalogo.Domain.Entities;
using DDD.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace DDD.Catalogo.Data.Repository
{
    internal class ProdutoRepository : IProdutoRepository
    {
        private readonly CatalogoContext _context;

        public ProdutoRepository(CatalogoContext catalogoContext)
        {
            _context = catalogoContext;
        }

        public IUnitOfWork UnitOfWork => _context;

        public void Adicionar(Produto produto) => _context.Produtos.Add(produto);

        public void Adicionar(Categoria categoria) => _context.Categorias.Add(categoria);

        public void Atualizar(Produto produto) => _context.Produtos.Update(produto);

        public void Atualizar(Categoria categoria) => _context.Categorias.Update(categoria);

        public async Task<IEnumerable<Categoria>> ObterCategorias()
        {
            return await _context.Categorias.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Produto>> ObterPorCategoria(int codigo)
        {
            return await _context.Produtos.AsNoTracking().Include(p => p.Categoria)
                .Where(c => c.Categoria.Codigo == codigo).ToListAsync();
        }

        public async Task<Produto> ObterPorId(Guid id)
        {
            return await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Produto>> ObterTodos()
        {
            return await _context.Produtos.AsNoTracking().ToListAsync();
        }

        public void Dispose() => _context?.Dispose();
    }
}
