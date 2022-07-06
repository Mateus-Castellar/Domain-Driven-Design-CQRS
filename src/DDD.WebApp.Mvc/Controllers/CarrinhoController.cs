using DDD.Catalogo.Application.Services;
using DDD.Core.Communication.Mediator;
using DDD.Core.Messages.CommonMessages.Notifications;
using DDD.Vendas.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DDD.WebApp.Mvc.Controllers
{
    public class CarrinhoController : CoreController
    {
        private readonly IProdutoService _produtoService;
        private readonly IMediatorHandler _mediatrHandler;

        public CarrinhoController(INotificationHandler<DomainNotification> notifications,
                                  IProdutoService produtoService,
                                  IMediatorHandler mediatrHandler) : base(notifications, mediatrHandler)
        {
            _produtoService = produtoService;
            _mediatrHandler = mediatrHandler;
        }

        public IActionResult Index()
        {
            return View();
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

            var command = new AdicionarItemPedidoCommand(ClientId, produto.Id, produto.Nome, quantidade, produto.Valor);
            await _mediatrHandler.EnviarComando(command);

            if (OperacaoValida())
                return RedirectToAction("Index");

            //caso de errado
            TempData["Erros"] = ObterMensagensErro();
            return RedirectToAction("ProdutoDetalhe", "Vitrine", new { id });
        }
    }
}