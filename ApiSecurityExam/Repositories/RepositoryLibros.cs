using ApiSecurityExam.Data;
using ApiSecurityExam.Models;
using Microsoft.EntityFrameworkCore;
using MvcCoreAzureStorage.Services;

namespace ApiSecurityExam.Repositories
{
    public class RepositoryLibros
    {
        private LibrosContext context;
        private ServiceStorageBlobs service;
        public RepositoryLibros(LibrosContext context)
        {
            this.service = service;
            this.context = context;
        }
        public async Task<List<Libros>> GetLibrosAsync()
        {
            return await this.context.Libros.ToListAsync();
        }
        public async Task<Libros> FindLibroAsync(int idLibro)
        {
            return await this.context.Libros.Where(z => z.IdLibro == idLibro).FirstOrDefaultAsync();
        }
        public async Task<List<Libros>> FindLibrosGenero(int idGenero)
        {
            return await this.context.Libros.Where(z => z.IdGenero == idGenero).ToListAsync();
        }
        public async Task CreateLibroAsync(int idLibro, string titulo, string autor,
            string editorial, string portada, int precio, int idGenero)
        {
            Libros libro = new Libros
            {
                IdLibro = idLibro,
                Titulo = titulo,
                Autor = autor,
                Editorial = editorial,
                Portada = portada,
                Precio = precio,
                IdGenero = idGenero
            };
            await this.context.Libros.AddAsync(libro);
            await this.context.SaveChangesAsync();
        }
        public async Task UpdateLibroAsync(int idLibro, string titulo, string autor,
            string editorial, string portada, int precio, int idGenero)
        {

            Libros libro = await this.context.Libros.FindAsync(idLibro);
            if (libro == null)
            {

                libro.IdLibro = idLibro;
                libro.Titulo = titulo;
                libro.Autor = autor;
                libro.Editorial = editorial;
                libro.Portada = portada;
                libro.Precio = precio;
                libro.IdGenero = idGenero;

            }

            await this.context.SaveChangesAsync();
        }
        public async Task DeleteLibroAsync(int idLibro)
        {
            Libros libro = await this.FindLibroAsync(idLibro);
            this.context.Libros.Remove(libro);
            await this.context.SaveChangesAsync();
        }
        public async Task<List<Generos>> GetGenerosAsync()
        {
            return await this.context.Generos.ToListAsync();
        }
        public async Task<Generos> FindGenero(int idGenero)
        {
            return await this.context.Generos.Where(z => z.IdGenero == idGenero).FirstOrDefaultAsync();
        }
        public async Task<List<Usuarios>> GetUsuariosAsync()
        {
            return await this.context.Usuarios.ToListAsync();
        }
        public async Task<int> GetMaxIdUsuario()
        {
            if (this.context.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await this.context.Usuarios.MaxAsync(z => z.IdUsuario) + 1;
            }

        }

        public async Task CreateUsuarioAsync(int idUsuario, string nombre, string apellido, string pass, string foto)
        {
            Usuarios usuario = new Usuarios
            {
                IdUsuario = await this.GetMaxIdUsuario(),
                Nombre = nombre,
                Apellido = apellido,
                Pass = pass,
                Foto = foto
            };
            await this.context.Usuarios.AddAsync(usuario);
            await this.context.SaveChangesAsync();
        }
        public async Task<Usuarios> LoginUsuario(string nombre, string pass)
        {
            return await this.context.Usuarios.Where(z => z.Nombre == nombre && z.Pass == pass).FirstOrDefaultAsync();
        }
        public async Task<int> GetMaxIdFactura()
        {
            var consulta = from datos in this.context.Pedidos
                           select datos;
            if (this.context.Pedidos.Any())
            {
                return await this.context.Pedidos.MaxAsync(z => z.IdFactura);
            }
            else
            {
                return 0;
            }
        }
        public async Task<int> GetMaxIdPedidoAsync()
        {
            var consulta = from datos in this.context.Pedidos
                           select datos;
            if (this.context.Pedidos.Any())
            {
                return await this.context.Pedidos.MaxAsync(z => z.IdPedido);
            }
            else
            {
                return 0;
            }
        }
        public async Task CreatePedidoAsync(int idUsuario, List<int> idLibros)
        {
            int nuevaFactura = await this.GetMaxIdFactura() + 1;
            DateTime fechaActual = DateTime.Now;

            List<Pedido> librosPedido = new List<Pedido>();
            foreach (int idLibro in idLibros)
            {
                Pedido libro = new Pedido
                {
                    IdLibro = idLibro,
                    IdFactura = nuevaFactura,
                    IdPedido = await this.GetMaxIdPedidoAsync() + 1,
                    Fecha = fechaActual,
                    IdUsuario = idUsuario,
                    Cantidad = 1
                };
                this.context.Pedidos.Add(libro);
                await this.context.SaveChangesAsync();
            }
        }
        public async Task<Usuarios> GetPerfilAsync(int idUsuario)
        {
            return await this.context.Usuarios.Where(z => z.IdUsuario == idUsuario).FirstOrDefaultAsync();
        }
        public async Task<List<Libros>> GetLibrosBlobAsync()
        {
            List<Libros> libros = await this.context.Libros.ToListAsync();
            string containerUrl = this.service.GetContainerUrl("libros");

            foreach (Libros libro in libros)
            {
                if (!libro.Portada.StartsWith("http"))
                {
                    string imagePath = libro.Portada;
                    if (!imagePath.StartsWith("PORTADAS/"))
                    {
                        imagePath = "PORTADAS/" + imagePath;
                    }

                    libro.Portada = containerUrl + "/" + imagePath;
                }
            }

            return libros;
        }

        public async Task<UserModel> PerfilUsuarioBlobAsync(int id)
        {
            Usuarios usuario = await this.context.Usuarios.Where(x => x.IdUsuario == id).FirstOrDefaultAsync();

            UserModel model = new UserModel
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Foto = usuario.Foto,
                Apellido = usuario.Apellido,
            };

            if (!string.IsNullOrEmpty(usuario.Foto))
            {
                string containerUrl = this.service.GetContainerUrl("cubos");

                if (!usuario.Foto.StartsWith("http"))
                {
                    string imagePath = usuario.Foto;
                    if (!imagePath.StartsWith("USUARIOS/"))
                    {
                        imagePath = "USUARIOS/" + imagePath;
                    }

                    model.Foto = containerUrl + "/" + imagePath;
                }
                else
                {
                    model.Foto = usuario.Foto;
                }
            }

            return model;
        }
    }
}
