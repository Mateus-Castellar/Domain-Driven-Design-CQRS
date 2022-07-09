using DDD.Core.Messages;
using FluentValidation;

namespace DDD.Vendas.Application.Commands
{
    public class AplicarCupomPedidoCommand : Command
    {
        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }
        public string CodigoCupom { get; private set; }

        public AplicarCupomPedidoCommand(Guid clienteId,
                                          Guid pedidoId,
                                          string codigoCupom)
        {
            ClienteId = clienteId;
            PedidoId = pedidoId;
            CodigoCupom = codigoCupom;
        }

        public override bool EhValido()
        {
            ValidationResult = new AplicarCupomPedidoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AplicarCupomPedidoValidation : AbstractValidator<AplicarCupomPedidoCommand>
    {
        public AplicarCupomPedidoValidation()
        {
            RuleFor(c => c.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do cliente inválido");

            RuleFor(c => c.PedidoId)
               .NotEqual(Guid.Empty)
               .WithMessage("Id do pedido inválido");

            RuleFor(c => c.CodigoCupom)
                .NotEmpty()
                .WithMessage("O código do cupom não pode estar vazio");
        }
    }
}