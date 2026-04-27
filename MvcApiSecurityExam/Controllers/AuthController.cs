using Microsoft.AspNetCore.Mvc;
using MvcApiSecurityExam.Models;
using MvcApiSecurityExam.Services;

namespace MvcApiSecurityExam.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiClientService api;

        public AuthController(ApiClientService api)
        {
            this.api = api;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("token") is not null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string? token = await this.api.LoginAsync(model);
            if (string.IsNullOrWhiteSpace(token))
            {
                ModelState.AddModelError(string.Empty, "Credenciales incorrectas");
                return View(model);
            }

            HttpContext.Session.SetString("token", token);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("token");
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
