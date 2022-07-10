using DDD.Pagamentos.Business.Entities;

namespace DDD.Pagamentos.Business.Interfaces
{
    public interface IPagamentoCartaoCreditoFacade
    {
        Transacao RealizarPagamento(Pedido pedido, Pagamento pagamento);
    }
}
