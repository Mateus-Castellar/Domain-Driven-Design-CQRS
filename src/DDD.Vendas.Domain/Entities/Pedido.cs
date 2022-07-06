using DDD.Core.DomainObjects;

namespace DDD.Vendas.Domain.Entities
{
    public class Pedido : Entity, IAggregateRoot
    {
        public int Codigo { get; private set; }
        public Guid ClienteId { get; private set; }
        public Guid? CupomId { get; private set; }
        public bool CupomUtilizado { get; private set; }
        public decimal Desconto { get; private set; }
        public decimal ValorTotal { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public PedidoStatus PedidoStatus { get; private set; }

        private readonly List<PedidoItem> _pedidoItems;
        public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItems;

        //Ef Core Relation
        public Cupom Cupom { get; private set; }

        protected Pedido()
        {
            _pedidoItems = new();
        }

        public Pedido(Guid clienteId,
                      bool cupomUtilizado,
                      decimal desconto,
                      decimal valorTotal)
        {
            ClienteId = clienteId;
            CupomUtilizado = cupomUtilizado;
            Desconto = desconto;
            ValorTotal = valorTotal;
            _pedidoItems = new();
        }

        public bool PedidoItemExistente(PedidoItem pedidoitem) =>
            _pedidoItems.Any(p => p.ProdutoId == pedidoitem.ProdutoId);

        public void CalcularValorPedido()
        {
            ValorTotal = PedidoItems.Sum(p => p.CalcularValor());
            CalcularValorTotalComDesconto();
        }

        public void AplicarCupom(Cupom cupom)
        {
            Cupom = cupom;
            CupomUtilizado = true;
            CalcularValorPedido();
        }

        public void CalcularValorTotalComDesconto()
        {
            if (CupomUtilizado is false)
                return;

            decimal desconto = 0;
            var valor = ValorTotal;

            if (Cupom.TipoDescontoCupom == TipoDescontoCupom.Porcentagem)
            {
                if (Cupom.Percentual.HasValue)
                {
                    desconto = (valor * Cupom.Percentual.Value) / 100;
                    valor -= desconto;
                }
            }
            else
            {
                if (Cupom.ValorDesconto.HasValue)
                {
                    desconto = Cupom.ValorDesconto.Value;
                    valor -= desconto;
                }
            }

            ValorTotal = valor < 0 ? 0 : valor;
            Desconto = desconto;
        }

        public void AdicionarItem(PedidoItem item)
        {
            if (item.EhValido() is false)
                return;

            item.AssociarPedido(Id);

            if (PedidoItemExistente(item))
            {
                var itemExistente = _pedidoItems.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);
                itemExistente.AdicionarUnidades(item.Quantidade);
                item = itemExistente;

                _pedidoItems.Remove(itemExistente);
            }

            item.CalcularValor();
            _pedidoItems.Add(item);
            CalcularValorPedido();
        }

        public void RemoverItem(PedidoItem item)
        {
            if (item.EhValido() is false)
                return;

            var itemExistente = PedidoItems.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);

            if (itemExistente is null)
                throw new DomainException("O item não pertence ao seu pedido");

            _pedidoItems.Remove(itemExistente);
            CalcularValorPedido();
        }

        public void AtualizarItem(PedidoItem item)
        {
            if (item.EhValido() is false)
                return;

            var itemExistente = PedidoItems.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);

            if (itemExistente is null)
                throw new DomainException("O item não pertence ao seu pedido");

            _pedidoItems.Remove(itemExistente);
            _pedidoItems.Add(item);
            CalcularValorPedido();
        }

        public void AtualizarUnidades(PedidoItem item, int unidades)
        {
            item.AtualizarUnidades(unidades);
            AtualizarItem(item);
        }

        public void TornarRascunho() => PedidoStatus = PedidoStatus.Rascunho;

        public void IniciarPedido() => PedidoStatus = PedidoStatus.Iniciado;

        public void FinalizarPedido() => PedidoStatus = PedidoStatus.Pago;

        public void CancelarPedido() => PedidoStatus = PedidoStatus.Cancelado;

        //classe aninhada..
        public static class PedidoFactory
        {
            public static Pedido NovoPedidoRascunho(Guid clienteId)
            {
                return new Pedido
                {
                    ClienteId = clienteId,
                    PedidoStatus = PedidoStatus.Rascunho
                };
            }
        }
    }
}