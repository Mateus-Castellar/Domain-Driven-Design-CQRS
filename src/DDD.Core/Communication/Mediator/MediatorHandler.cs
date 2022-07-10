using DDD.Core.Data.EventSourcing;
using DDD.Core.Messages;
using DDD.Core.Messages.CommonMessages.Notifications;
using MediatR;

namespace DDD.Core.Communication.Mediator
{
    public interface IMediatorHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;
        Task<bool> EnviarComando<T>(T comando) where T : Command;
        Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification;
    }

    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;
        private readonly IEventSourcingRepository _eventSourcingRepository;

        public MediatorHandler(IMediator mediator,
                               IEventSourcingRepository eventSourcingRepository)
        {
            _mediator = mediator;
            _eventSourcingRepository = eventSourcingRepository;
        }

        //Publish é um evento/notificacao (nem sempre possui inteção de mudança)..
        public async Task PublicarEvento<T>(T evento) where T : Event
        {
            await _mediator.Publish(evento);

            if (evento.GetType().BaseType.Name.Equals("DomainEvent") is false)
                await _eventSourcingRepository.SalvarEvento(evento);
        }

        //Send é um request, envia algo que afetara a app de alguma forma..
        public async Task<bool> EnviarComando<T>(T comando) where T : Command =>
            await _mediator.Send(comando);

        //envia uma notificação..
        public async Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification =>
            await _mediator.Publish(notificacao);
    }
}
