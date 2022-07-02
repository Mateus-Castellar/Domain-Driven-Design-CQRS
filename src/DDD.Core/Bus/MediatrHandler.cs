using DDD.Core.Messages;
using MediatR;

namespace DDD.Core.Bus
{
    public interface IMediatrHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;
    }

    public class MediatrHandler : IMediatrHandler
    {
        private readonly IMediator _mediator;

        public MediatrHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublicarEvento<T>(T evento) where T : Event
        {
            //lancando o evento com o mediator (alguem precisa pegar esse evento !!)
            await _mediator.Publish(evento);
        }
    }
}
