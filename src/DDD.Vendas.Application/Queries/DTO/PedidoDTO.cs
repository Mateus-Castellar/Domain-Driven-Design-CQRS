namespace DDD.Vendas.Application.Queries.DTO
{
    public class PedidoDTO
    {
        public int Codigo { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime DataCadastro { get; set; }
        public int PedidoStatus { get; set; }
    }
}