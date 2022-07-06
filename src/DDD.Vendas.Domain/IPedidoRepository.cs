using DDD.Core.Data;
using DDD.Vendas.Domain.Entities;

namespace DDD.Vendas.Domain
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        //pedidos
        Task<Pedido> ObterPorId(Guid id);
        Task<IEnumerable<Pedido>> ObterListaPorClienteId(Guid clienteId);
        Task<Pedido> ObterPedidoRascunhoPorClienteId(Guid clienteId);
        void Adicionar(Pedido pedido);
        void Atualizar(Pedido pedido);

        //pedido itens
        Task<PedidoItem> ObterItemPorId(Guid id);
        Task<PedidoItem> ObterItemPorPedido(Guid pedidoId, Guid produtoId);
        void AdicionarItem(PedidoItem pedidoItem);
        void AtualizarItem(PedidoItem pedidoItem);
        void RemoverItem(PedidoItem pedidoItem);

        //cupom
        Task<Cupom> ObterCupomPorCodigo(string codigo);
    }
}