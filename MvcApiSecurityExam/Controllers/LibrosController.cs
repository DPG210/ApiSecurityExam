using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcApiSecurityExam.Models;
using MvcApiSecurityExam.Services;

namespace MvcApiSecurityExam.Controllers
{
    public class LibrosController : Controller
    {
        private readonly ApiClientService api;

        public LibrosController(ApiClientService api)
        {
            this.api = api;
        }

        private async Task LoadGenerosAsync(int? selectedId = null)
        {
            List<Generos> generos = await this.api.GetGenerosAsync();
            ViewBag.Generos = new SelectList(generos, "IdGenero", "Nombre", selectedId);
        }

        private async Task LoadLibrosAsync()
        {
            ViewBag.Libros = await this.api.GetLibrosAsync();
        }

        private async Task LoadUsuariosAsync()
        {
            ViewBag.Usuarios = await this.api.GetUsuariosAsync();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Libros> libros = await this.api.GetLibrosAsync();
            ViewBag.Generos = await this.api.GetGenerosAsync();
            return View(libros);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Libros? libro = await this.api.GetLibroAsync(id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        [HttpGet]
        public async Task<IActionResult> LibrosGenero(int idGenero)
        {
            List<Libros> libros = await this.api.GetLibrosGeneroAsync(idGenero);
            ViewBag.Generos = await this.api.GetGenerosAsync();
            ViewBag.IdGenero = idGenero;
            return View(libros);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadGenerosAsync();
            return View(new Libros());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Libros model)
        {
            if (!ModelState.IsValid)
            {
                await LoadGenerosAsync(model.IdGenero);
                return View(model);
            }

            await this.api.CreateLibroAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Libros? libro = await this.api.GetLibroAsync(id);
            if (libro == null)
            {
                return NotFound();
            }

            await LoadGenerosAsync(libro.IdGenero);
            return View(libro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Libros model)
        {
            if (!ModelState.IsValid)
            {
                await LoadGenerosAsync(model.IdGenero);
                return View(model);
            }

            await this.api.UpdateLibroAsync(id, model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Libros? libro = await this.api.GetLibroAsync(id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await this.api.DeleteLibroAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Generos()
        {
            List<Generos> generos = await this.api.GetGenerosAsync();
            return View(generos);
        }

        [HttpGet]
        public async Task<IActionResult> Genero(int idGenero)
        {
            Generos? genero = await this.api.GetGeneroAsync(idGenero);
            if (genero == null)
            {
                return NotFound();
            }

            return View(genero);
        }

        [HttpGet]
        public async Task<IActionResult> Usuarios()
        {
            List<Usuarios> usuarios = await this.api.GetUsuariosAsync();
            return View(usuarios);
        }

        [HttpGet]
        public IActionResult CreateUsuario()
        {
            return View(new Usuarios());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUsuario(Usuarios model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await this.api.CreateUsuarioAsync(model);
            return RedirectToAction(nameof(Usuarios));
        }

        [HttpGet]
        public async Task<IActionResult> CreatePedido()
        {
            await LoadUsuariosAsync();
            await LoadLibrosAsync();
            return View(new PedidoRequestModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePedido(PedidoRequestModel model)
        {
            if (!ModelState.IsValid || model.IdLibros.Count == 0)
            {
                await LoadUsuariosAsync();
                await LoadLibrosAsync();
                ModelState.AddModelError(string.Empty, "Selecciona al menos un libro");
                return View(model);
            }

            await this.api.CreatePedidoAsync(model.IdUsuario, model.IdLibros);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Perfil()
        {
            UserModel? model = await this.api.GetPerfilUsuarioAsync();
            if (model == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> PerfilBlob()
        {
            UserModel? model = await this.api.GetPerfilUsuarioBlobAsync();
            if (model == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LibrosBlob()
        {
            List<Libros> libros = await this.api.GetLibrosBlobAsync();
            return View(libros);
        }
    }
}
