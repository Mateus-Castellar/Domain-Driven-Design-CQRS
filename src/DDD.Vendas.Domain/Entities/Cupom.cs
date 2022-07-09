using DDD.Core.DomainObjects;
using FluentValidation;
using FluentValidation.Results;

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

        internal ValidationResult ValidarSeAplicavel()
        {
            return new CupomAplicavelValidation().Validate(this);
        }

    }

    public class CupomAplicavelValidation : AbstractValidator<Cupom>
    {
        public CupomAplicavelValidation()
        {
            RuleFor(c => c.DataValidade)
                .Must(DataVencimentoSuperioAtual)
                .WithMessage("O cupom está expirado");

            RuleFor(c => c.Ativo)
                .Equal(true)
                .WithMessage("O cupom não é mais válido");

            RuleFor(c => c.Utilizado)
                .Equal(false)
                .WithMessage("Esse cupom já foi utilizado");

            RuleFor(c => c.Quantidade)
                .GreaterThan(0)
                .WithMessage("O cupom não esta mais disponível para uso");
        }

        protected static bool DataVencimentoSuperioAtual(DateTime dataValidade) => dataValidade >= DateTime.Now;
    }
}