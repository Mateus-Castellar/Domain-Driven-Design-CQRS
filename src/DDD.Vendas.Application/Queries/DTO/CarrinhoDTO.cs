namespace DDD.Vendas.Application.Queries.DTO
{
    public class CarrinhoDTO
    {
        public Guid PedidoId { get; set; }
        public Guid ClienteId { get; set; }
        public decimal SubTotal { get; set; }//valor total - desconto
        public decimal ValorTotal { get; set; }
        public decimal ValorDesconto { get; set; }
        public string CupomCodigo { get; set; }

        public List<CarrinhoItemDTO> Items { get; set; } = new();
        public CarrinhoPagamentoDTO Pagamento { get; set; }

    }
}