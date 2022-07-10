using DDD.Catalogo.Domain.Services;
using DDD.Core.Communication.Mediator;
using DDD.Core.Messages.CommonMessages.IntegrationEvents;
using MediatR;

namespace DDD.Catalogo.Domain.Events
{
    public class ProdutoEventHandler :
        INotificationHandler<ProdutoEstoqueAbaixoEvent>,
        INotificationHandler<PedidoIniciadoEvent>
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IEstoqueService _estoqueService;
        private readonly IMediatorHandler _mediatorHandler;

        public ProdutoEventHandler(IProdutoRepository produtoRepository,
                                   IEstoqueService estoqueService,
                                   IMediatorHandler mediatorHandler)
        {
            _produtoRepository = produtoRepository;
            _estoqueService = estoqueService;
            _mediatorHandler = mediatorHandler;
        }

        public async Task Handle(ProdutoEstoqueAbaixoEvent message, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.ObterPorId(message.AggregateId);
            //enviar um email ou solicitar um servico para comprar mais unidades de estoque
        }

        public async Task Handle(PedidoIniciadoEvent message, CancellationToken cancellationToken)
        {
            var sucesso = await _estoqueService.DebitarListaProdutosPedido(message.ProdutosPedidos);

            if (sucesso)
                await _mediatorHandler.PublicarEvento(new PedidoEstoqueConfirmadoEvent(message.ClienteId, message.ClienteId, message.Total,
                    message.ProdutosPedidos, message.NomeCartao, message.NumeroCartao, message.ExpiracaoCartao, message.CvvCartao));
            else
                await _mediatorHandler.PublicarEvento(new PedidoEstoqueRejeitadoEvent(message.PedidoId, message.ClienteId));
        }
    }
}