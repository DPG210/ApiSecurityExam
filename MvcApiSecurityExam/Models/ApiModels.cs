using System.ComponentModel.DataAnnotations;

namespace MvcApiSecurityExam.Models
{
    public class Libros
    {
        public int IdLibro { get; set; }

        [Required]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Autor { get; set; } = string.Empty;

        [Required]
        public string Editorial { get; set; } = string.Empty;

        [Required]
        public string Portada { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int Precio { get; set; }

        [Range(1, int.MaxValue)]
        public int IdGenero { get; set; }
    }

    public class Generos
    {
        public int IdGenero { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;
    }

    public class Usuarios
    {
        public int IdUsuario { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Apellido { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string Pass { get; set; } = string.Empty;

        public string? Foto { get; set; }
    }

    public class LoginModel
    {
        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class UserModel
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Foto { get; set; }
    }

    public class PedidoRequestModel
    {
        [Range(1, int.MaxValue)]
        public int IdUsuario { get; set; }

        [MinLength(1)]
        public List<int> IdLibros { get; set; } = new();
    }

    public class ApiTokenResponse
    {
        public string? Response { get; set; }
    }
}
