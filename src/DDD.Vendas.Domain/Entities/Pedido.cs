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

        private readonly List<Pedidoitem> _pedidoitems;
        public IReadOnlyCollection<Pedidoitem> Pedidoitems => _pedidoitems;

        //Ef Core Relation
        public Cupom Cupom { get; private set; }

        protected Pedido()
        {
            _pedidoitems = new();
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
            _pedidoitems = new();
        }

        public bool PedidoItemExistente(Pedidoitem pedidoitem) =>
            _pedidoitems.Any(p => p.ProdutoId == pedidoitem.ProdutoId);

        public void CalcularValorPedido()
        {
            ValorTotal = Pedidoitems.Sum(p => p.CalcularValor());
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

        public void AdicionarItem(Pedidoitem item)
        {
            if (item.EhValido() is false)
                return;

            item.AssociarPedido(Id);

            if (PedidoItemExistente(item))
            {
                var itemExistente = _pedidoitems.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);
                itemExistente.AdicionarUnidades(item.Quantidade);
                item = itemExistente;

                _pedidoitems.Remove(itemExistente);
            }

            item.CalcularValor();
            _pedidoitems.Add(item);
            CalcularValorPedido();
        }

        public void RemoverItem(Pedidoitem item)
        {
            if (item.EhValido() is false)
                return;

            var itemExistente = Pedidoitems.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);

            if (itemExistente is null)
                throw new DomainException("O item não pertence ao seu pedido");

            _pedidoitems.Remove(itemExistente);
            CalcularValorPedido();
        }

        public void AtualizarItem(Pedidoitem item)
        {
            if (item.EhValido() is false)
                return;

            var itemExistente = Pedidoitems.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);

            if (itemExistente is null)
                throw new DomainException("O item não pertence ao seu pedido");

            _pedidoitems.Remove(itemExistente);
            _pedidoitems.Add(item);
            CalcularValorPedido();
        }

        public void AtualizarUnidades(Pedidoitem item, int unidades)
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