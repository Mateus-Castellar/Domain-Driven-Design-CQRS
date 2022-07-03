namespace DDD.Vendas.Domain.Entities
{
    public enum PedidoStatus
    {
        Rascunho = 0,
        Iniciado = 1,

        //intervalo para adicionar possiveis status antes do pago

        Pago = 4,
        Entregue = 5,
        Cancelado = 6
    }
}