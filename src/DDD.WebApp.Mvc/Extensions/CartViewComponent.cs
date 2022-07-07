using DDD.Vendas.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace DDD.WebApp.Mvc.Extensions
{
    public class CartViewComponent : ViewComponent
    {
        private readonly IPedidosQueries _pedidosQueries;

        //TODO: Capturar cliente logado
        protected Guid ClienteId = Guid.Parse("748D51E8-C13F-4A9F-9227-3B25B016D063");

        public CartViewComponent(IPedidosQueries pedidosQueries)
        {
            _pedidosQueries = pedidosQueries;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var carrinho = await _pedidosQueries.ObterCarrinhoCliente(ClienteId);
            var itens = carrinho?.Items.Count ?? 0;

            return View(itens);
        }
    }
}