using DDD.Core.Messages;
using MediatR;

namespace DDD.Core.Bus
{
    public interface IMediatorHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;
        Task<bool> EnviarComando<T>(T comando) where T : Command;
    }

    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        //Publish é um evento/notificacao (nem sempre possui inteção de mudança)..
        public async Task PublicarEvento<T>(T evento) where T : Event => await _mediator.Publish(evento);

        //Send é um request, envia algo que afetara a app de alguma forma..
        public async Task<bool> EnviarComando<T>(T comando) where T : Command => await _mediator.Send(comando);

    }
}
