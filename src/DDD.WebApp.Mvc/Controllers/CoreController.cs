using DDD.Core.Communication.Mediator;
using DDD.Core.Messages.CommonMessages.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DDD.WebApp.Mvc.Controllers
{
    public abstract class CoreController : Controller
    {
        protected Guid ClienteId = Guid.Parse("748D51E8-C13F-4A9F-9227-3B25B016D063");

        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatorHandler _mediatorHandler;

        public CoreController(INotificationHandler<DomainNotification> notifications,
                              IMediatorHandler mediatorHandler)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _mediatorHandler = mediatorHandler;
        }

        protected bool OperacaoValida() => _notifications.TemNotificacoes() is false;

        protected IEnumerable<string> ObterMensagensErro() =>
            _notifications.ObterNotificacoes().Select(lbda => lbda.Value).ToList();

        protected void NotificarErro(string codigo, string mensagem) =>
            _mediatorHandler.PublicarNotificacao(new DomainNotification(codigo, mensagem));
    }
}