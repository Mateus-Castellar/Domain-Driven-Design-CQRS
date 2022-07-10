using DDD.Core.Communication.Mediator;
using DDD.Core.DomainObjects.DTO;
using DDD.Core.Extensions;
using DDD.Core.Messages;
using DDD.Core.Messages.CommonMessages.IntegrationEvents;
using DDD.Core.Messages.CommonMessages.Notifications;
using DDD.Vendas.Application.Events;
using DDD.Vendas.Domain;
using DDD.Vendas.Domain.Entities;
using MediatR;

namespace DDD.Vendas.Application.Commands
{
    public class PedidoCommandHandler : IRequestHandler<AdicionarItemPedidoCommand, bool>,
                                        IRequestHandler<AtualizarItemPedidoCommand, bool>,
                                        IRequestHandler<RemoverItemPedidoCommand, bool>,
                                        IRequestHandler<AplicarCupomPedidoCommand, bool>,
                                        IRequestHandler<IniciarPedidoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public PedidoCommandHandler(IPedidoRepository pedidoRepository,
                                    IMediatorHandler mediatorHandler)
        {
            _pedidoRepository = pedidoRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(AdicionarItemPedidoCommand message, CancellationToken cancellationToken)
        {
            if (ValidarComando(message) is false)
                return false;

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClienteId);
            var pedidoItem = new PedidoItem(message.ProdutoId, message.Nome, message.Quantidade, message.ValorUnitario);

            if (pedido is null)
            {
                //se e um novo pedido...
                pedido = Pedido.PedidoFactory.NovoPedidoRascunho(message.ClienteId);
                pedido.AdicionarItem(pedidoItem);

                _pedidoRepository.Adicionar(pedido);//coloca o dado em memoria no EfCore
                pedido.AdicionarEvento(new PedidoRascunhoIniciadoEvent(message.ClienteId, pedido.Id));
            }
            else
            {
                //se o pedido ja existe...
                var pedidoItemExistente = pedido.PedidoItemExistente(pedidoItem);
                pedido.AdicionarItem(pedidoItem);

                if (pedidoItemExistente)
                    _pedidoRepository.AtualizarItem(pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId));
                else
                    _pedidoRepository.AdicionarItem(pedidoItem);

                pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            }

            pedido.AdicionarEvento(new PedidoItemAdicionadoEvent(pedido.ClienteId, pedido.Id, message.ProdutoId,
                message.Nome, message.ValorUnitario, message.Quantidade));

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(AtualizarItemPedidoCommand message, CancellationToken cancellationToken)
        {
            if (ValidarComando(message) is false)
                return false;

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClienteId);

            if (pedido is null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado!"));
                return false;
            }

            var pedidoItem = await _pedidoRepository.ObterItemPorPedido(pedido.Id, message.ProdutoId);

            if (pedido.PedidoItemExistente(pedidoItem) is false)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Item não encontrado no pedido"));
                return false;
            }

            pedido.AtualizarUnidades(pedidoItem, message.Quantidade);
            pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            pedido.AdicionarEvento(new PedidoProdutoAtualizadoEvent(message.ClienteId, pedido.Id, message.ProdutoId, message.Quantidade));

            _pedidoRepository.AtualizarItem(pedidoItem);
            _pedidoRepository.Atualizar(pedido);

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(RemoverItemPedidoCommand message, CancellationToken cancellationToken)
        {
            if (ValidarComando(message) is false)
                return false;

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClienteId);

            if (pedido is null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Pedido não encontrado!"));
                return false;
            }

            var pedidoItem = await _pedidoRepository.ObterItemPorPedido(pedido.Id, message.ProdutoId);

            if (pedido.PedidoItemExistente(pedidoItem) is false && pedidoItem is not null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "Item não encontrado no pedido"));
                return false;
            }

            pedido.RemoverItem(pedidoItem);
            pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            pedido.AdicionarEvento(new PedidoProdutoRemovidoEvent(message.ClienteId, pedido.Id, message.ProdutoId));

            _pedidoRepository.RemoverItem(pedidoItem);
            _pedidoRepository.Atualizar(pedido);

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(AplicarCupomPedidoCommand message, CancellationToken cancellationToken)
        {
            if (ValidarComando(message) is false)
                return false;

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClienteId);

            if (pedido is null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "pedido não encontrado"));
                return false;
            }

            var cupom = await _pedidoRepository.ObterCupomPorCodigo(message.CodigoCupom);

            if (cupom is null)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("pedido", "cupom não encontrado"));
                return false;
            }

            var CupomAplicacaoValidation = pedido.AplicarCupom(cupom);

            if (CupomAplicacaoValidation.IsValid is false)
            {
                foreach (var erro in CupomAplicacaoValidation.Errors)
                    await _mediatorHandler.PublicarNotificacao(new DomainNotification(erro.ErrorCode, erro.ErrorMessage));
            }

            pedido.AdicionarEvento(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            pedido.AdicionarEvento(new CupomAplicadoPedidoEvent(message.ClienteId, pedido.Id, cupom.Id));

            //TODO: implementar Regra para debitar no banco o cupom

            _pedidoRepository.Atualizar(pedido);

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(IniciarPedidoCommand message, CancellationToken cancellationToken)
        {
            if (ValidarComando(message) is false)
                return false;

            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClienteId);
            pedido.IniciarPedido(); //muda o status do pedido para iniciado

            var itensList = new List<Item>();
            pedido.PedidoItems.ForEach(lbda => itensList.Add(new Item { Id = lbda.Id, Quantidade = lbda.Quantidade }));
            var listaProdutosPedido = new ListaProdutosPedido { PedidoId = pedido.Id, Itens = itensList, };

            pedido.AdicionarEvento(new PedidoIniciadoEvent(pedido.Id, pedido.ClienteId, pedido.ValorTotal, listaProdutosPedido,
                message.NomeCartao, message.NumeroCartao, message.ExpiracaoCartao, message.CvvCartao));

            _pedidoRepository.Atualizar(pedido);
            return await _pedidoRepository.UnitOfWork.Commit();
        }

        private bool ValidarComando(Command message)
        {
            if (message.EhValido())
                return true;

            foreach (var error in message.ValidationResult.Errors)
            {
                //lancar um evento de erro (não é uma exception)  
                _mediatorHandler.PublicarNotificacao(new DomainNotification(message.MessageType, error.ErrorMessage));
            }

            return false;
        }
    }
}