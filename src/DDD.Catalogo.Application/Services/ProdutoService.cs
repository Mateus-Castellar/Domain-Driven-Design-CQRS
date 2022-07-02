using AutoMapper;
using DDD.Catalogo.Application.DTO;
using DDD.Catalogo.Domain;
using DDD.Catalogo.Domain.Entities;
using DDD.Catalogo.Domain.Services;
using DDD.Core.DomainObjects;

namespace DDD.Catalogo.Application.Services
{
    public interface IProdutoService : IDisposable
    {
        Task<IEnumerable<ProdutoDTO>> ObterPorCategoria(int codigo);
        Task<ProdutoDTO> ObterPorId(Guid id);
        Task<IEnumerable<ProdutoDTO>> ObterTodos();
        Task<IEnumerable<CategoriaDTO>> ObterCategorias();

        Task AdicionarProduto(ProdutoDTO produtoDTO);
        Task AtualizarProduto(ProdutoDTO produtoDTO);
        Task<ProdutoDTO> DebitarEstoque(Guid id, int quantidade);
        Task<ProdutoDTO> ReporEstoque(Guid id, int quantidade);
    }

    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IEstoqueService _estoqueService;
        private readonly IMapper _mapper;

        public ProdutoService(IProdutoRepository produtoRepository, IEstoqueService estoqueService, IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _estoqueService = estoqueService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProdutoDTO>> ObterPorCategoria(int codigo)
        {
            return _mapper.Map<IEnumerable<ProdutoDTO>>(await _produtoRepository.ObterPorCategoria(codigo));
        }

        public async Task<ProdutoDTO> ObterPorId(Guid id)
        {
            return _mapper.Map<ProdutoDTO>(await _produtoRepository.ObterPorId(id));
        }

        public async Task<IEnumerable<ProdutoDTO>> ObterTodos()
        {
            return _mapper.Map<IEnumerable<ProdutoDTO>>(await _produtoRepository.ObterTodos());
        }

        public async Task<IEnumerable<CategoriaDTO>> ObterCategorias()
        {
            return _mapper.Map<IEnumerable<CategoriaDTO>>(await _produtoRepository.ObterCategorias());
        }

        public async Task AdicionarProduto(ProdutoDTO produtoDTO)
        {
            _produtoRepository.Adicionar(_mapper.Map<Produto>(produtoDTO));
            await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task AtualizarProduto(ProdutoDTO produtoDTO)
        {
            _produtoRepository.Atualizar(_mapper.Map<Produto>(produtoDTO));
            await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task<ProdutoDTO> DebitarEstoque(Guid id, int quantidade)
        {
            if (_estoqueService.DebitarEstoque(id, quantidade).Result is false)
                throw new DomainException("Falha ao debitar estoque");

            return _mapper.Map<ProdutoDTO>(await _produtoRepository.ObterPorId(id));
        }

        public async Task<ProdutoDTO> ReporEstoque(Guid id, int quantidade)
        {
            if (_estoqueService.ReporEstoque(id, quantidade).Result is false)
                throw new DomainException("Falha ao repor estoque");

            return _mapper.Map<ProdutoDTO>(await _produtoRepository.ObterPorId(id));
        }

        public void Dispose() => _produtoRepository?.Dispose();
    }
}