using DDD.Core.Data;
using DDD.Pagamentos.Business.Entities;

namespace DDD.Pagamentos.Business.Interfaces
{
    public interface IPagamentoRepository : IRepository<Pagamento>
    {
        void Adicionar(Pagamento pagamento);
        void AdicionarTransacao(Transacao transacao);
    }
}
