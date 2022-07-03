using DDD.Catalogo.Application.DTO;
using DDD.Catalogo.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DDD.WebApp.Mvc.Controllers.Admin
{
    public class AdminProdutosController : Controller
    {
        private readonly IProdutoService _produtoService;

        public AdminProdutosController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet]
        [Route("admin-produtos")]
        public async Task<IActionResult> Index() => View(await _produtoService.ObterTodos());

        [HttpGet]
        [Route("novo-produto")]
        public async Task<IActionResult> NovoProduto()
        {
            return View(await PopularCategorias(new ProdutoDTO()));
        }

        [HttpPost]
        [Route("novo-produto")]
        public async Task<IActionResult> NovoProduto(ProdutoDTO produtoDTO)
        {
            if (ModelState.IsValid is false)
                return View(await PopularCategorias(produtoDTO));

            await _produtoService.AdicionarProduto(produtoDTO);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("editar-produto")]
        public async Task<IActionResult> AtualizarProduto(Guid id)
        {
            return View(await PopularCategorias(await _produtoService.ObterPorId(id)));
        }

        [HttpPost]
        [Route("editar-produto")]
        public async Task<IActionResult> AtualizarProduto(Guid id, ProdutoDTO produtoViewModel)
        {
            var produto = await _produtoService.ObterPorId(id);
            produtoViewModel.QuantidadeEstoque = produto.QuantidadeEstoque;

            ModelState.Remove("QuantidadeEstoque");

            if (ModelState.IsValid is false)
                return View(await PopularCategorias(produtoViewModel));

            await _produtoService.AtualizarProduto(produtoViewModel);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("produtos-atualizar-estoque")]
        public async Task<IActionResult> AtualizarEstoque(Guid id)
        {
            return View("Estoque", await _produtoService.ObterPorId(id));
        }

        [HttpPost]
        [Route("produtos-atualizar-estoque")]
        public async Task<IActionResult> AtualizarEstoque(Guid id, int quantidade)
        {
            if (quantidade > 0)
                await _produtoService.ReporEstoque(id, quantidade);
            else
                await _produtoService.DebitarEstoque(id, quantidade);

            return View("Index", await _produtoService.ObterTodos());
        }

        private async Task<ProdutoDTO> PopularCategorias(ProdutoDTO produto)
        {
            produto.Categorias = await _produtoService.ObterCategorias();
            return produto;
        }
    }
}