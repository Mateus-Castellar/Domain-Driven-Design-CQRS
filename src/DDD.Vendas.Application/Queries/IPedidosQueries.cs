using DDD.Vendas.Application.Queries.DTO;

namespace DDD.Vendas.Application.Queries
{
    public interface IPedidosQueries
    {
        Task<CarrinhoDTO> ObterCarrinhoCliente(Guid clienteId);
        Task<IEnumerable<PedidoDTO>> ObterPedidosCliente(Guid clienteId);
    }
}