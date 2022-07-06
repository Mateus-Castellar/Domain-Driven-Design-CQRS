using DDD.Catalogo.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace DDD.WebApp.Mvc.Controllers
{
    public class VitrineController : CoreController
    {
        private readonly IProdutoService _produtoService;

        public VitrineController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet]
        [Route("")]
        [Route("vitrine")]
        public async Task<IActionResult> Index() => View(await _produtoService.ObterTodos());

        [HttpGet]
        [Route("produto-detalhe/{id}")]
        public async Task<IActionResult> ProdutoDetalhe(Guid id) => View(await _produtoService.ObterPorId(id));
    }
}