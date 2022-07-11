using DDD.Vendas.Application.Queries.DTO;
using DDD.Vendas.Domain;
using DDD.Vendas.Domain.Entities;

namespace DDD.Vendas.Application.Queries
{
    public class PedidosQueries : IPedidosQueries
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidosQueries(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<CarrinhoDTO> ObterCarrinhoCliente(Guid clienteId)
        {
            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(clienteId);

            if (pedido is null)
                return null;

            var carrinho = new CarrinhoDTO
            {
                ClienteId = pedido.ClienteId,
                ValorTotal = pedido.ValorTotal,
                PedidoId = pedido.Id,
                ValorDesconto = pedido.Desconto,
                SubTotal = pedido.Desconto + pedido.ValorTotal
            };

            if (pedido.CupomId is not null)
                carrinho.CupomCodigo = pedido.Cupom.Codigo;

            foreach (var item in pedido.PedidoItems)
            {
                carrinho.Items.Add(new CarrinhoItemDTO
                {
                    ProdutoId = item.ProdutoId,
                    ProdutoNome = item.ProdutoNome,
                    Quantidade = item.Quantidade,
                    ValorUnitario = item.ValorUnitario,
                    ValorTotal = item.ValorUnitario * item.Quantidade
                });
            }

            return carrinho;
        }

        public async Task<IEnumerable<PedidoDTO>> ObterPedidosCliente(Guid clienteId)
        {
            var pedidos = await _pedidoRepository.ObterListaPorClienteId(clienteId);

            if (pedidos is null)
                return null;

            pedidos = pedidos.Where(p => p.PedidoStatus == PedidoStatus.Pago || p.PedidoStatus == PedidoStatus.Cancelado)
                .OrderByDescending(p => p.Codigo);

            if (pedidos.Any() is false)
                return null;

            var pedidosView = new List<PedidoDTO>();

            foreach (var pedido in pedidos)
            {
                pedidosView.Add(new PedidoDTO
                {
                    Id = pedido.Id,
                    Codigo = pedido.Codigo,
                    DataCadastro = pedido.DataCadastro,
                    PedidoStatus = (int)pedido.PedidoStatus,
                    ValorTotal = pedido.ValorTotal
                });
            }

            return pedidosView;
        }
    }
}