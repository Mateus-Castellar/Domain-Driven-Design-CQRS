using DDD.Core.Communication.Mediator;
using DDD.Core.Messages.CommonMessages.Notifications;
using DDD.Vendas.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DDD.WebApp.Mvc.Controllers
{
    public class PedidoController : CoreController
    {
        private readonly IPedidosQueries _pedidoQueries;

        public PedidoController(IPedidosQueries pedidoQueries,
                                INotificationHandler<DomainNotification> notifications,
                                IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
        {
            _pedidoQueries = pedidoQueries;
        }

        [Route("meus-pedidos")]
        public async Task<IActionResult> Index() => View(await _pedidoQueries.ObterPedidosCliente(ClienteId));

    }
}
