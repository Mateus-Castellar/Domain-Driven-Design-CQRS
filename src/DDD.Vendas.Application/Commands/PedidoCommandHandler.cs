using DDD.Core.Messages;
using MediatR;

namespace DDD.Vendas.Application.Commands
{
    public class PedidoCommandHandler : IRequestHandler<AdicionarItemPedidoCommand, bool>
    {
        public async Task<bool> Handle(AdicionarItemPedidoCommand message, CancellationToken cancellationToken)
        {
            if (ValidarComando(message) is false)
                return false;

            return true;
        }

        private static bool ValidarComando(Command message)
        {
            if (message.EhValido())
                return true;

            foreach (var error in message.ValidationResult.Errors)
            {
                //lancar um evento de erro (não é uma exception)
            }

            return false;
        }
    }
}
