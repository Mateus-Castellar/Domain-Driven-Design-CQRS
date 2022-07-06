using Microsoft.AspNetCore.Mvc;

namespace DDD.WebApp.Mvc.Controllers
{
    public abstract class CoreController : Controller
    {
        protected Guid ClientId = Guid.Parse("748D51E8-C13F-4A9F-9227-3B25B016D063");
    }
}