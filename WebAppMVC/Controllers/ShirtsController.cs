using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Data;
using WebAppMVC.Models;
using WebAppMVC.Models.Repositories;

namespace WebAppMVC.Controllers
{
    public class ShirtsController : Controller
    {
        private readonly IWebApiExecutor _webApiExecutor;

        public ShirtsController(IWebApiExecutor webApiExecutor)
        {
            _webApiExecutor = webApiExecutor;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _webApiExecutor.InvokeGet<List<Shirt>>("shirts"));
        }

        public IActionResult CreateShirt()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateShirt(Shirt shirt)
        {
            if(ModelState.IsValid)
            {
                var response = await _webApiExecutor.InvokePost("shirts", shirt);
                if (response != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(shirt);
        }
    }
}
