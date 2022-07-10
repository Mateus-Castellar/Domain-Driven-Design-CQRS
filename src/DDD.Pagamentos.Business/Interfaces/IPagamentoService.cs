using DDD.Core.DomainObjects.DTO;
using DDD.Pagamentos.Business.Entities;

namespace DDD.Pagamentos.Business.Interfaces
{
    public interface IPagamentoService
    {
        Task<Transacao> RealizarPagamentoPedido(PagamentoPedido pagamentoPedido);
    }
}