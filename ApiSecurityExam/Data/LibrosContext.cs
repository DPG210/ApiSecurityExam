using ApiSecurityExam.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiSecurityExam.Data
{
    public class LibrosContext : DbContext
    {
        public LibrosContext(DbContextOptions options) : base(options) { }
        public DbSet<Libros> Libros { get; set; }
        public DbSet<Generos> Generos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<VistaPedidos> VistaPedidos { get; set; }
    }
}
