using DDD.Core.DomainObjects;

namespace DDD.Vendas.Domain.Entities
{
    public class PedidoItem : Entity
    {
        public Guid PedidoId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        //Ef Core Relation
        public Pedido Pedido { get; private set; }
        protected PedidoItem() { }

        public PedidoItem(Guid produtoId,
                          string produtoNome,
                          int quantidade,
                          decimal valorUnitario)
        {
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        public override bool EhValido() => true;

        internal void AssociarPedido(Guid pedidoId) => PedidoId = pedidoId;

        public decimal CalcularValor() => Quantidade * ValorUnitario;

        internal void AdicionarUnidades(int unidades) => Quantidade += unidades;

        internal void AtualizarUnidades(int unidades) => Quantidade = unidades;
    }
}