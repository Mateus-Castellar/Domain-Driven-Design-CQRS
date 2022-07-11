using DDD.Catalogo.Application.Services;
using DDD.Core.Communication.Mediator;
using DDD.Core.Messages.CommonMessages.Notifications;
using DDD.Vendas.Application.Commands;
using DDD.Vendas.Application.Queries;
using DDD.Vendas.Application.Queries.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DDD.WebApp.Mvc.Controllers
{
    public class CarrinhoController : CoreController
    {
        private readonly IProdutoService _produtoService;
        private readonly IMediatorHandler _mediatrHandler;
        private readonly IPedidosQueries _pedidoQueries;

        public CarrinhoController(INotificationHandler<DomainNotification> notifications,
                                  IProdutoService produtoService,
                                  IPedidosQueries pedidoQueries,
                                  IMediatorHandler mediatrHandler) : base(notifications, mediatrHandler)
        {
            _produtoService = produtoService;
            _pedidoQueries = pedidoQueries;
            _mediatrHandler = mediatrHandler;
        }

        [Route("meu-carrinho")]
        public async Task<IActionResult> Index()
        {
            return View(await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
        }

        [HttpPost]
        [Route("meu-carrinho")]
        public async Task<IActionResult> AdicionarItem(Guid id, int quantidade)
        {
            var produto = await _produtoService.ObterPorId(id);

            if (produto is null)
                return BadRequest();

            if (produto.QuantidadeEstoque < quantidade)
            {
                TempData["Erro"] = "Produto com estoque insuficiente";
                return RedirectToAction("ProdutoDetalhe", "Vitrine", new { id });
            }

            var command = new AdicionarItemPedidoCommand(ClienteId, produto.Id, produto.Nome, quantidade, produto.Valor);
            await _mediatrHandler.EnviarComando(command);

            if (OperacaoValida())
                return RedirectToAction("Index");

            //caso de errado
            TempData["Erros"] = ObterMensagensErro();
            return RedirectToAction("ProdutoDetalhe", "Vitrine", new { id });
        }

        [HttpPost]
        [Route("remover-item")]
        public async Task<IActionResult> RemoverItem(Guid id)
        {
            var produto = await _produtoService.ObterPorId(id);

            if (produto is null)
                return BadRequest();

            var command = new RemoverItemPedidoCommand(ClienteId, id);
            await _mediatrHandler.EnviarComando(command);

            if (OperacaoValida())
                return RedirectToAction("Index");

            return View("Index", await _pedidoQueries.ObterCarrinhoCliente(ClienteId));

        }

        [HttpPost]
        [Route("atualizar-item")]
        public async Task<IActionResult> AtualizarItem(Guid id, int quantidade)
        {
            var produto = await _produtoService.ObterPorId(id);

            if (produto is null)
                return BadRequest();

            var command = new AtualizarItemPedidoCommand(ClienteId, id, quantidade);
            await _mediatrHandler.EnviarComando(command);

            if (OperacaoValida())
                return RedirectToAction("Index");

            return View("Index", await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
        }

        [HttpPost]
        [Route("aplicar-cupom")]
        public async Task<IActionResult> AplicarCupom(string cupomCodigo)
        {
            var command = new AplicarCupomPedidoCommand(ClienteId, cupomCodigo);
            await _mediatrHandler.EnviarComando(command);

            if (OperacaoValida())
                return RedirectToAction("Index");

            return View("Index", await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
        }

        [HttpGet]
        [Route("resumo-da-compra")]
        public async Task<IActionResult> ResumoDaCompra()
        {
            var carrinhoCliente = await _pedidoQueries.ObterCarrinhoCliente(ClienteId);
            return View(carrinhoCliente);
        }

        [HttpPost]
        [Route("iniciar-pedido")]
        public async Task<IActionResult> IniciarPedido(CarrinhoDTO carrinhoDTO)
        {
            var carrinho = await _pedidoQueries.ObterCarrinhoCliente(ClienteId);

            var command = new IniciarPedidoCommand(carrinho.PedidoId, ClienteId, carrinho.ValorTotal, carrinhoDTO.Pagamento.NomeCartao,
                 carrinhoDTO.Pagamento.NumeroCartao, carrinhoDTO.Pagamento.ExpiracaoCartao, carrinhoDTO.Pagamento.CvvCartao);

            await _mediatrHandler.EnviarComando(command);

            if (OperacaoValida())
                return RedirectToAction("Index", "Pedido");

            var carrinhoCompleto = await _pedidoQueries.ObterCarrinhoCliente(ClienteId);
            return View("ResumoDaCompra", carrinhoCompleto);
        }
    }
}