using DDD.Core.DomainObjects;

namespace DDD.Vendas.Domain.Entities
{
    public class Cupom : Entity
    {
        public string Codigo { get; private set; }
        public decimal? Percentual { get; private set; }
        public decimal? ValorDesconto { get; private set; }
        public int Quantidade { get; private set; }
        public TipoDescontoCupom TipoDescontoCupom { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataUtilizacao { get; private set; }
        public DateTime DataValidade { get; private set; }
        public bool Ativo { get; private set; }
        public bool Utilizado { get; private set; }

        //Ef Core Relation (1 cupom pode ser usado em varios pedidos)
        public ICollection<Pedido> Pedidos { get; set; }
    }
}