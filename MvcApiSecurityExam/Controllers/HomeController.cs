using Microsoft.AspNetCore.Mvc;
using MvcApiSecurityExam.Models;
using MvcApiSecurityExam.Services;
using System.Diagnostics;

namespace MvcApiSecurityExam.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiClientService api;

        public HomeController(ApiClientService api)
        {
            this.api = api;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalLibros = (await this.api.GetLibrosAsync()).Count;
            ViewBag.TotalGeneros = (await this.api.GetGenerosAsync()).Count;
            ViewBag.TotalUsuarios = (await this.api.GetUsuariosAsync()).Count;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
