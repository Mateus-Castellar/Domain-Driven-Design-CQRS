using DDD.Catalogo.Domain.Events;
using DDD.Core.Communication.Mediator;

namespace DDD.Catalogo.Domain.Services
{
    public interface IEstoqueService : IDisposable
    {
        Task<bool> DebitarEstoque(Guid produtoId, int quantidade);
        Task<bool> ReporEstoque(Guid produtoId, int quantidade);
    }

    public class EstoqueService : IEstoqueService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMediatorHandler _bus;

        public EstoqueService(IProdutoRepository produtoRepository, IMediatorHandler bus)
        {
            _produtoRepository = produtoRepository;
            _bus = bus;
        }

        public async Task<bool> DebitarEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _produtoRepository.ObterPorId(produtoId);

            if (produto is null)
                return false;

            if (produto.VerficarEstoque(quantidade) is false)
                return false;

            produto.DebitarEstoque(quantidade);

            if (produto.QuantidadeEstoque <= 10)
            {
                //avisar que a quantidade de estoque esta baixa..
                await _bus.PublicarEvento(new ProdutoEstoqueAbaixoEvent(produto.Id, produto.QuantidadeEstoque));
            }

            //atualiza a quantidade no banco...
            _produtoRepository.Atualizar(produto);

            return await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> ReporEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _produtoRepository.ObterPorId(produtoId);

            if (produto is null)
                return false;

            produto.ReporEstoques(quantidade);

            _produtoRepository.Atualizar(produto);

            return await _produtoRepository.UnitOfWork.Commit();
        }

        public void Dispose() => _produtoRepository?.Dispose();
    }
}
