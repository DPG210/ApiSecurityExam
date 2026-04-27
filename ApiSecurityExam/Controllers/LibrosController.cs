using ApiSecurityExam.Helpers;
using ApiSecurityExam.Models;
using ApiSecurityExam.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApiSecurityExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private RepositoryLibros repo;
        private HelperUsuarioToken helper;
        public LibrosController(RepositoryLibros repo, HelperUsuarioToken helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> Libros()
        {
            List<Libros> libros = await this.repo.GetLibrosAsync();
            return Ok(libros);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult> Libro(int id)
        {
            Libros libro = await this.repo.FindLibroAsync(id);
            return Ok(libro);
        }

        [HttpGet]
        [Route("[action]/{idGenero}")]
        public async Task<ActionResult> LibrosGenero(int idGenero)
        {
            List<Libros> libros = await this.repo.FindLibrosGenero(idGenero);
            return Ok(libros);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> CreateLibro(Libros model)
        {
            await this.repo.CreateLibroAsync(model.IdLibro, model.Titulo, model.Autor,
                model.Editorial, model.Portada, model.Precio, model.IdGenero);
            return Ok();
        }

        [HttpPut]
        [Route("[action]/{id}")]
        public async Task<ActionResult> UpdateLibro(int id, Libros model)
        {
            await this.repo.UpdateLibroAsync(id, model.Titulo, model.Autor,
                model.Editorial, model.Portada, model.Precio, model.IdGenero);
            return Ok();
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<ActionResult> DeleteLibro(int id)
        {
            await this.repo.DeleteLibroAsync(id);
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> Generos()
        {
            List<Generos> generos = await this.repo.GetGenerosAsync();
            return Ok(generos);
        }

        [HttpGet]
        [Route("[action]/{idGenero}")]
        public async Task<ActionResult> Genero(int idGenero)
        {
            Generos genero = await this.repo.FindGenero(idGenero);
            return Ok(genero);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult> Usuarios()
        {
            List<Usuarios> usuarios = await this.repo.GetUsuariosAsync();
            return Ok(usuarios);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> CreateUsuario(Usuarios model)
        {
            await this.repo.CreateUsuarioAsync(model.IdUsuario, model.Nombre, model.Apellido,
                model.Pass, model.Foto);
            return Ok();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            Usuarios usuario = await this.repo.LoginUsuario(model.Nombre, model.Password);
            return Ok(usuario);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> CreatePedido(int idUsuario, List<int> idLibros)
        {
            await this.repo.CreatePedidoAsync(idUsuario, idLibros);
            return Ok();
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<UserModel>> PerfilUsuario()
        {
            UserModel model = this.helper.GetUsuario();
            return model;
        }

        [HttpGet("PerfilBlob")]
        public async Task<ActionResult<UserModel>> PerfilUsuarioBlob()
        {

            UserModel model = this.helper.GetUsuario();
            var userblob = await this.repo.PerfilUsuarioBlobAsync(model.IdUsuario);
            return userblob;
        }
        [HttpGet("LibrosBlob")]
        public async Task<ActionResult<List<Libros>>> GetLibrosBlob()
        {
            return await this.repo.GetLibrosBlobAsync();
        }
    }
}
