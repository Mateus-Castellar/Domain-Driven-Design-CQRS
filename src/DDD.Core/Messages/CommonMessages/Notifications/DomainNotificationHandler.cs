using MediatR;

namespace DDD.Core.Messages.CommonMessages.Notifications
{
    public class DomainNotificationHandler : INotificationHandler<DomainNotification>
    {
        private List<DomainNotification> _notifications;

        public DomainNotificationHandler()
        {
            _notifications = new();
        }

        public Task Handle(DomainNotification message, CancellationToken cancellationToken)
        {
            _notifications.Add(message);
            return Task.CompletedTask;
        }

        public virtual List<DomainNotification> ObterNotificacoes() => _notifications;

        public virtual bool TemNotificacoes() => ObterNotificacoes().Any();

        public void Dispose() => _notifications = new();
    }
}